using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Pingresp : Packet
    {
        public Pingresp()
        {
            PacketType = (int) MQTTPacketTypes.PINGRESP;
            CompletePacket(null);
        }
    }
}
