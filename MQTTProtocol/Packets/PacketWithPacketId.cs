using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public abstract class PacketWithPacketId : Packet
    {
        public int PacketId { get; protected set; }
    }
}
