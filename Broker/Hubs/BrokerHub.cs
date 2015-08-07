using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BrokerApp
{
    [HubName("BrokerHub")]
    public class BrokerHub : Hub
    {
        public BrokerHub()
        {
            
        }

        public override Task OnConnected()
        {
            SendClientList();
            return base.OnConnected();
        }

        public void SendClientList()
        {
            Clients.All.sendClientList(BrokerConnections.ClientCollection.ToArray());
        }

        public void NewClientConnected(MQTTClient client)
        {
            Clients.All.newClientConnected(client);
        }

        public void ClientDisconnected(MQTTClient client)
        {
            Clients.All.clientDisconnected(client);
        }

        public void BrokerDisconnecting()
        {
            Clients.All.brokerDisconnecting();
        }

        public void NewPacketRecieved(bool recieved, int clientId, string mqttPacketType, string data, string datetime)
        {
            Clients.All.newPacketRecieved(recieved, clientId, mqttPacketType, data, datetime);
        }

        public void ShutdownBroker()
        {
            Broker.ShutDownBroker();
        }

        public void AddSubscription(int clientId, string topic)
        {
            Clients.All.addSubscription(clientId, topic);
        }

        public void RemoveSubscription(int clientId, string topic)
        {
            Clients.All.removeSubscription(clientId, topic);
        }
    }
}
