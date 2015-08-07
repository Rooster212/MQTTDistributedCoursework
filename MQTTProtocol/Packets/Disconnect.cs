using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Disconnect : Packet
    {
        public Disconnect()
        {
            PacketType = (int) MQTTPacketTypes.DISCONNECT;
            CompletePacket(null);
        }
    }
}
