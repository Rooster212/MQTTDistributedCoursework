using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Puback : PacketWithPacketId
    {
        public Puback()
        {
            PacketType = (byte)MQTTPacketTypes.PUBACK;
            CompletePacket(null);
        }

        public Puback(int packetId)
        {
            PacketId = packetId;
            PacketType = (byte)MQTTPacketTypes.PUBACK;
            List<byte> tempPacket = new List<byte>
            {
                getMsb(packetId), 
                getLsb(packetId)
            };
            CompletePacket(tempPacket);
        }
    }
}
