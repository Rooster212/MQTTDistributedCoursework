using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using MQTTProtocol;
using MQTTProtocol.Packets;

namespace BrokerApp
{
    class BrokerConnections : MQTTBase
    {
        private CustomTcpListener listener { get; set; }
        private Thread listenThread;

        private bool _runBroker = false;

        private static int clientCounter = 0;
        public static ConcurrentDictionary<int,MQTTClient> ClientCollection = new ConcurrentDictionary<int,MQTTClient>();

        public void StartServer()
        {
            
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
            _runBroker = true;
        }

        private void ListenForClients()
        {
            listener = new CustomTcpListener(IPAddress.Any, int.Parse(ConfigurationManager.AppSettings["mqttPort"]));
            try
            {
                if (!listener.Active)
                    listener.Start();
            }
            catch (Exception ex)
            {
                // no idea why this breaks sometimes but not always
                Trace.WriteLine(ex.Message);
            }

            while (_runBroker)
            {
                TcpClient client = null;

                try
                {
                    // this avoids the blocking call for the listener so we can safely end the thread
                    if (listener.Pending())
                        client = listener.AcceptTcpClient();
                    else
                    {
                        Thread.Sleep(THREAD_SLEEP_REST_TIME);
                        continue;
                    }

                }
                catch (SocketException)
                {
                    //welp
                }

                if (client != null)
                {
                    var ipAddressOfClient = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                    MQTTClient mqttClient = new MQTTClient()
                    {
                        UniqueId = ++clientCounter,
                        TCPClient = client,
                        IPAddress = ipAddressOfClient,
                        LastContactTime = DateTime.Now
                    };

                    Thread clientThread = new Thread(() => HandleClient(mqttClient));
                    clientThread.Start();
                }
            }
        }

        private void HandleClient(MQTTClient client)
        {
            var tcpClient = client.TCPClient;
            NetworkStream stream = tcpClient.GetStream();
            while (_runBroker)
            {
                try
                {
                    try
                    {
                        int buffer;
                        if ((buffer = stream.ReadByte()) != -1)
                        {

                            bool disconnect = false;
                            List<byte> bufferList = new List<byte>();
                            do
                            {
                                bufferList.Add((byte) buffer);
                                if (stream.DataAvailable)
                                    buffer = stream.ReadByte();
                                else break;
                            } while (buffer != -1);
                            var response = DoRecieve(client, bufferList, out disconnect);
                            if (disconnect || !_runBroker)
                            {
                                break;
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (response != null)
                            {
                                var hubContext = GlobalHost.ConnectionManager.GetHubContext<BrokerHub>();
                                client.LastContactTime = DateTime.Now;
                                stream.Write(response.PacketBytes, 0, response.PacketBytes.Length);
                                hubContext.Clients.All.newPacketRecieved(false, client.UniqueId, Enum.GetName(typeof(MQTTPacketTypes), response.PacketType), string.Empty, DateTime.Now.ToLongTimeString());
                                Console.WriteLine(Enum.GetName(typeof(MQTTPacketTypes), response.PacketType)+ " sent");
                                stream.Flush();
                            }
                            Console.ResetColor();
                        }
                    }
                    catch (IOException ex)
                    {
                        // if the ReceiveTimeout is reached an IOException will be raised with ErrorCode 10060
                        var socketException = ex.InnerException as SocketException;
                        if (socketException == null || socketException.ErrorCode != 10060)
                            throw; // if it's not the "expected" exception, let's not hide the error
                        // if it is the receive timeout, then reading ended
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine("Error in Broker communication handle mqttClient: " + e.Message);
                    Console.WriteLine("Error in Broker communication handle mqttClient: " + e.Message);
                }
            }
            stream.Close();
            tcpClient.Close();
        }
        private Packet DoRecieve(MQTTClient mqttClient, IEnumerable<byte> dataBytes, out bool disconnect)
        {
            // declare hub
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BrokerHub>();

            List<byte> byteList = dataBytes.ToList();
            MQTTPacketTypes mqttPacketType = (MQTTPacketTypes)byteList.First();
            disconnect = false;
            string data = string.Empty;
            Packet responsePacket = null;
            Console.ForegroundColor = ConsoleColor.Red;
            switch (mqttPacketType)
            {
                // types we should recieve
                case MQTTPacketTypes.PUBLISH:
                    Console.WriteLine("PUBLISH recieved");
                    var pubPacket = new Publish(byteList);
                    PublishToAllClients(pubPacket.TopicName, pubPacket.Payload);
                    data = string.Format("Topic: {0} Data: {1}", pubPacket.TopicName, pubPacket.Payload);
                    // responsePacket = new Puback();
                    break;
                case MQTTPacketTypes.DISCONNECT:
                    Console.WriteLine("DISCONNECT recieved");
                    disconnect = true; // handle at the higher level
                    MQTTClient temp;
                    if (!ClientCollection.TryRemove(mqttClient.UniqueId, out temp))
                    {
                        //Trace.WriteLine("Error occurred removing client from client collection");
                    }
                    hubContext.Clients.All.clientDisconnected(mqttClient.UniqueId);
                    break;
                case MQTTPacketTypes.PINGREQ:
                    Console.WriteLine("PINGREQ recieved");
                    var pingRespPacket = new Pingresp();
                    responsePacket = pingRespPacket;
                    break;
                case MQTTPacketTypes.UNSUBSCRIBE:
                    Console.WriteLine("UNSUBSCRIBE recieved");
                    var unsubPacket = new Unsubscribe((List<byte>)dataBytes);

                    string summaryString = string.Empty;
                    foreach (var topic in unsubPacket.TopicNames)
                    {
                        hubContext.Clients.All.removeSubscription(mqttClient.UniqueId, topic);
                        summaryString += topic + " ";
                    }

                    data = string.Format("Topics: {0}",summaryString);

                    // remove all subscriptions in unsubscribe
                    mqttClient.SubscriptionList = mqttClient.SubscriptionList.Except(unsubPacket.TopicNames).ToList();

                    var unsubackPacket = new Unsuback(unsubPacket.PacketId);
                    responsePacket = unsubackPacket;
                    break;
                case MQTTPacketTypes.SUBSCRIBE:
                    Console.WriteLine("SUBSCRIBE recieved");
                    var subPacket = new Subscribe((List<byte>)dataBytes);
                    
                    List<int> subSuccess = new List<int>();
                    foreach (var item in subPacket.subList)
                    {
                        if(!mqttClient.SubscriptionList.Contains(item.Name))
                        {
                            mqttClient.SubscriptionList.Add(item.Name);
                            subSuccess.Add(1);
                        }
                        else
                            subSuccess.Add(0);
                    }
                    string summaryString2 = string.Empty;
                    foreach (var topic in subPacket.subList)
                    {
                        hubContext.Clients.All.addSubscription(mqttClient.UniqueId, topic.Name);
                        summaryString2 += topic.Name + " ";
                    }

                    data = string.Format("Topics: {0}",summaryString2);

                    var subAckPacket = new Suback(subPacket.PacketId, subSuccess.ToArray());
                    responsePacket = subAckPacket;
                    break;
                case MQTTPacketTypes.CONNECT:
                    Console.WriteLine("CONNECT recieved");
                    var connectPacket = new Connect((List<byte>)dataBytes);
                    mqttClient.ClientID = connectPacket.ClientId;
                    ClientCollection.GetOrAdd(clientCounter, mqttClient);
                    hubContext.Clients.All.newClientConnected(mqttClient);
                    responsePacket = new Connack();
                    break;
                // types we should NOT recieve
                case MQTTPacketTypes.CONNACK:
                    Console.WriteLine("CONNACK recieved");
                    break;
                case MQTTPacketTypes.SUBACK:
                    Console.WriteLine("SUBACK recieved");
                    break;
                case MQTTPacketTypes.PINGRESP:
                    Console.WriteLine("PINGRESP recieved");
                    break;
                case MQTTPacketTypes.UNSUBACK:
                    Console.WriteLine("UNSUBACK recieved");
                    break;
                default:
                    Console.WriteLine("Unknown packet recieved");
                    break;

            }
            Console.ResetColor();
            hubContext.Clients.All.newPacketRecieved(true, mqttClient.UniqueId, Enum.GetName(typeof(MQTTPacketTypes), mqttPacketType), data, DateTime.Now.ToLongTimeString());
            return responsePacket;
        }

        private void PublishToAllClients(string topicName, string data)
        {
            var pubPacket = new Publish(topicName, data);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BrokerHub>();
            Parallel.ForEach(ClientCollection.Where(i => i.Value.SubscriptionList.Contains(topicName)),
                item =>
                {
                    hubContext.Clients.All.newPacketRecieved(false,
                        item.Value.UniqueId,
                        Enum.GetName(typeof (MQTTPacketTypes), MQTTPacketTypes.PUBLISH),
                        String.Format("Topic: {0} Data: {1}", pubPacket.TopicName, pubPacket.Payload),
                        DateTime.Now.ToLongTimeString()
                        );
                    item.Value.TCPClient.GetStream().Write(pubPacket.PacketBytes, 0, pubPacket.PacketBytes.Length);
                    item.Value.TCPClient.GetStream().Flush();
                }
            );
        }

        public void Shutdown()
        {
            _runBroker = false;
            Parallel.ForEach(ClientCollection, i =>
            {
                i.Value.TCPClient.Close();
            });
        }

    }
}
