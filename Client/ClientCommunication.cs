using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Timer = System.Timers.Timer;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Client.MQTTWcfService;
using MQTTProtocol;
using MQTTProtocol.Packets;

namespace Client
{
    internal class ClientCommunication : MQTTBase
    {
        #region properties

        /// <summary>
        /// Signifies whether there is a currently open connection.
        /// </summary>
        public bool Connected { get; private set; }

        #endregion

        #region private variables

        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly ConcurrentQueue<Packet> _packetSendQueue = new ConcurrentQueue<Packet>();
        private ClientForm _form;

        private Thread readThread;
        private Thread writeThread;
        private Thread pingThread;

        private bool AllowedToDisconnect { get; set; }
        #endregion

        public ClientCommunication(string host, int port, int timeout, ClientForm form, bool testing)
        {
            _form = form;
            _client = new TcpClient();
            var result = _client.BeginConnect(host, port, null, null);

            // start connection
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeout));

            // if successful
            if (success)
            {
                _stream = _client.GetStream();
                Connected = true;
                AllowedToDisconnect = false;
                readThread = new Thread(ReadHandler);
                writeThread = new Thread(WriteHandler);
                pingThread = new Thread(PingHandler);
                readThread.Start();
                writeThread.Start();
                pingThread.Start();
            }
            else
            {
                if(!testing)
                MessageBox.Show(String.Format("Timeout occurred connecting to server \"{0}\" on port {1}", host, port), @"Timeout");
            }
        }

        public void Disconnect()
        {
            while (Connected)
            {
                if (_packetSendQueue.Count == 0 && AllowedToDisconnect)
                {
                    _stream.Close();
                    _client.Close();
                    Connected = false;
                }
                else
                {
                    Thread.Sleep(THREAD_SLEEP_REST_TIME);
                }
            }

        }

        private void PingHandler()
        {
            var pingTimer = new Timer();
            pingTimer.Elapsed += new ElapsedEventHandler(AddPing);
            pingTimer.Interval = PING_INTERVAL_MS;
            pingTimer.Enabled = true;
        }

        private void AddPing(object source, ElapsedEventArgs e)
        {
            AddPacketToSendQueue(new Pingreq());
        }

        /// <summary>
        /// Adds a packet to the queue to be sent to the server.
        /// </summary>
        /// <param name="packetArray">PacketBytes byte array</param>
        public void AddPacketToSendQueue(params Packet[] packetArray)
        {
            foreach(var packet in packetArray)
                _packetSendQueue.Enqueue(packet);
        }

        private void ReadHandler()
        {
            NetworkStream stream = _client.GetStream();
            _client.Client.ReceiveTimeout = TCP_TIMEOUT;
            _client.Client.SendTimeout = TCP_TIMEOUT;
            try
            {
                // handle read

                while (Connected)
                {
                    int buffer;
                    try
                    {
                        if ((buffer = stream.ReadByte()) != -1)
                        {

                            List<byte> bufferList = new List<byte>();
                            do
                            {
                                bufferList.Add((byte)buffer);
                                if (stream.DataAvailable)
                                    buffer = stream.ReadByte();
                                else break;
                            } while (buffer != -1);

                            Thread recieveThread = new Thread(() => DoRecieve(bufferList));
                            recieveThread.Start();
                        }
                    }
                    catch (IOException ex)
                    {
                        //ErrorCode 10060 ReceiveTimeout is reached
                        //ErrorCode 10053 (Connection closed by local machine)
                        //ErrorCode 10054 (Connection closed by remote host)
                        //ErrorCode 10004 (Stream closed)
                        var socketException = ex.InnerException as SocketException;
                        var acceptableErrorCodes = new int[] {10060, 10053, 10054, 10004};
                        if (socketException != null)
                        {
                            switch (socketException.ErrorCode)
                            {
                                case 10060: // if it is the receive timeout, then reading ended
                                    break;
                                case 10053: // connection closed by local machine
                                    break;
                                case 10054: // connection closed by remote host
                                    break;
                                case 10004: // stream closed
                                    break;
                                default:
                                    throw; // if it's not the "expected" exception, let's not hide the error
                            }
                        }
                    }
                    Thread.Sleep(THREAD_SLEEP_REST_TIME);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(@"Exception occurred: " + e.Message);
            }
        }

        private void WriteHandler()
        {
            var stream = _client.GetStream();
            try
            {
                while (Connected)
                {
                    Packet packet;
                    if (_packetSendQueue.TryDequeue(out packet))
                    {
                        stream.ReadTimeout = 250;
                        stream.Write(packet.PacketBytes, 0, packet.PacketBytes.Length);
                        stream.Flush();

                        MQTTPacketTypes packetType = (MQTTPacketTypes)packet.PacketType;

                        switch (packetType)
                        {
                            case MQTTPacketTypes.CONNECT:
                                AddListViewItem(false, packetType, null, "", "", DateTime.Now);
                                break;
                            case MQTTPacketTypes.PUBLISH:
                                var publishPacket = (Publish)packet;
                                AddListViewItem(false, packetType, null, publishPacket.TopicName, publishPacket.Payload, DateTime.Now);
                                break;
                            case MQTTPacketTypes.SUBSCRIBE:
                                var subscribePacket = (Subscribe)packet;
                                StringBuilder subscribeString = new StringBuilder();
                                foreach (var item in subscribePacket.subList)
                                {
                                    subscribeString.Append(item.Name);
                                    subscribeString.Append(" ");
                                }
                                AddListViewItem(false, packetType, subscribePacket.PacketId, subscribeString.ToString(), "", DateTime.Now);
                                break;
                            case MQTTPacketTypes.UNSUBSCRIBE:
                                var unsubscribePacket = (Unsubscribe)packet;
                                StringBuilder unsubscribeString = new StringBuilder();
                                foreach (var item in unsubscribePacket.TopicNames)
                                {
                                    unsubscribeString.Append(item);
                                    unsubscribeString.Append(" ");
                                }
                                AddListViewItem(false, packetType, null, "", "", DateTime.Now);
                                break;
                            case MQTTPacketTypes.PINGREQ:
                            case MQTTPacketTypes.PINGRESP:
                                AddListViewItem(false, packetType, null, "", "", DateTime.Now);
                                break;
                            case MQTTPacketTypes.DISCONNECT:
                                AddListViewItem(false, packetType, null, "", "", DateTime.Now);
                                AllowedToDisconnect = true;
                                break;
                            default:
                                AddListViewItem(false, (MQTTPacketTypes)packet.PacketType, null, "", "", DateTime.Now);
                                break;
                        }

                    }
                    Thread.Sleep(THREAD_SLEEP_REST_TIME);
                }

            }
            catch (IOException e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private string GetStringForOutputSend(MQTTPacketTypes packetType)
        {
            switch (packetType)
            {
                case MQTTPacketTypes.CONNECT:
                    return "Connect request";
                    break;
                case MQTTPacketTypes.PUBLISH:
                    return "Publish request";
                    break;
                case MQTTPacketTypes.SUBSCRIBE:
                    return "Subscribe request";
                    break;
                case MQTTPacketTypes.UNSUBSCRIBE:
                    return "Unsubscribe request";
                    break;
                case MQTTPacketTypes.PINGREQ:
                    return "Ping request";
                    break;
                case MQTTPacketTypes.DISCONNECT:
                    return "Disconnect request";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("packetType");
            }
        }


        private void DoRecieve(List<byte> dataBytes)
        {
            MQTTPacketTypes mqttPacketType = (MQTTPacketTypes)dataBytes.First();
            string payload = string.Empty;
            string topic = string.Empty;
            int? packetId = null;

            switch (mqttPacketType)
            {

                case MQTTPacketTypes.CONNACK:
                    Connected = true;
                    payload = "Connection acknowledged";
                    break;
                case MQTTPacketTypes.PUBLISH:
                    var pubPacket = new Publish(dataBytes);
                    payload = pubPacket.Payload;
                    topic = pubPacket.TopicName;
                    break;
                case MQTTPacketTypes.PUBACK:
                    payload = "Publish acknowledged";
                    break;
                case MQTTPacketTypes.SUBACK:
                    var subackPacket = new Suback(dataBytes);
                    packetId = subackPacket.PacketId;
                    payload = "Subscription acknowledged";
                    break;
                case MQTTPacketTypes.UNSUBACK:
                    var unsubackPacket = new Unsuback(dataBytes);
                    packetId = unsubackPacket.PacketId;
                    payload = "Unsubscription acknowledged";
                    break;
                case MQTTPacketTypes.PINGRESP:
                    payload = "Ping response.";
                    break;

                // other packet types that our clients shouldn't recieve
                case MQTTPacketTypes.DISCONNECT:
                case MQTTPacketTypes.PINGREQ:
                case MQTTPacketTypes.UNSUBSCRIBE:
                case MQTTPacketTypes.SUBSCRIBE:
                case MQTTPacketTypes.CONNECT:
                default:
                    payload = "Invalid receive or unknown packet";
                    break;

            }
            AddListViewItem(true, mqttPacketType, packetId, topic, payload, DateTime.Now);
        }

        private void AddListViewItem(bool recieve, MQTTPacketTypes packetType, int? packetId, string topic, string data, DateTime time)
        {
            _form.AddListViewItem(recieve, packetType, packetId, topic, data, time);
        }

        public static string[] GetServers()
        {
            var directoryServiceClientRef = new DirectoryServiceClient();
            //directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.UserName = "423048";
            //directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.Password = "<redacted>";
            //directoryServiceClientRef.ClientCredentials.Windows.ClientCredential.Domain = "ADIR";
            return directoryServiceClientRef.GetServers();
        }
    }
}
