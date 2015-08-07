using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol
{
    public class Subscription
    {
        public string Name { get; set; }

        public int Qos { get; set; }

        public Subscription(string name, int qos)
        {
            this.Name = name;
            this.Qos = qos;
        }
    }
}
