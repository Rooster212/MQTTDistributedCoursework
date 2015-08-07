using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace BrokerApp
{
    public class MQTTClient
    {

        [JsonIgnore]
        public TcpClient TCPClient { get; set; }

        public int UniqueId { get; set; }

        public MQTTClient()
        {
            SubscriptionList = new List<string>();
            LastContactTime = DateTime.Now;
        }

        public DateTime LastContactTime { get; set; }

        public List<string> SubscriptionList { get; set; }

        public string IPAddress { get; set; }

        public string ClientID { get; set; }
    }
}
