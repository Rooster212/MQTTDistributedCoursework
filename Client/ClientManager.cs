using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MQTTProtocol.Packets;

namespace Client
{
    internal class ClientManager
    {
        public List<string> SubscribedTopicsList { get; set; }

        public ClientCommunication Communication { get; private set; }

        public ClientStorage Storage { get; set; }

        private bool _initiated = false;

        private int _nextPacketId = 0;

        private ClientForm _form;

        public int NextPacketId { get { return ++_nextPacketId; } }

        public void Init(string host, int port, string clientId, int timeout, ClientForm form, bool test)
        {
            try
            {
                SubscribedTopicsList = new List<string>();
                Communication = new ClientCommunication(host, port, timeout, form, test);
                _form = form;
                if (Communication.Connected)
                {
                    _initiated = true;
                    Communication.AddPacketToSendQueue(new Connect(clientId));
                    if (test)
                    {
                        const string topic1 = "a/b";
                        const string topic2 = "b/c";
                        const string topic3 = "c/d";
                        const string testPayload = "hello world";
                        Packet[] testPackets = new Packet[]
                        {
                            new Subscribe(0, topic1, topic2),
                            new Publish(topic1, testPayload),
                            new Publish(topic2, testPayload),
                            new Subscribe(1, topic3),
                            new Publish(topic3, String.Format("{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}",testPayload)),
                            new Publish(topic1, String.Format("{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}",testPayload, topic3+topic2)),
                            new Pingreq(),
                            new Unsubscribe(2,topic1),
                            new Subscribe(3,topic1),
                            new Publish(topic2, String.Format("{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}",testPayload, topic1+topic3)),
                            new Unsubscribe(4,topic1), 
                            new Unsubscribe(5,topic2), 
                            new Unsubscribe(6,topic3),
                            new Publish(topic2, String.Format("{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}{0}{1}",testPayload, topic1+topic3)),
                            new Pingreq()
                        };
                        Communication.AddPacketToSendQueue(testPackets);
                    }
                }
                else
                {
                    _initiated = false;
                }
                
            }
            catch (SocketException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void Disconnect(bool test = false)
        {
            Communication.AddPacketToSendQueue(new Disconnect());
            Communication.Disconnect();
            if(!test)
                _form.Invoke(new MethodInvoker(_form.ResetUiDisconnected));
        }
    }
}
