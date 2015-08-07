using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Pingreq : Packet
    {
        public Pingreq()
        {
            PacketType = (int) MQTTPacketTypes.PINGREQ;
            CompletePacket(null);
        }
    }
}
