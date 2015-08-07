using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Unsuback : PacketWithPacketId
    {
        public Unsuback(int packetId)
        {
            PacketId = packetId;
            PacketType = (int) MQTTPacketTypes.UNSUBACK;
            List<byte> tempPacket = new List<byte>();
            tempPacket.Add(getMsb(packetId));
            tempPacket.Add(getLsb(packetId));
            CompletePacket(tempPacket);
        }

        public Unsuback(List<byte> packetList)
        {
            packetList.RemoveAt(0);
            MemoryStream stream = new MemoryStream(packetList.ToArray());
            int remainingLengthOfPacket = decodeRemainingLength(ref stream);

            PacketId = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());
        }
    }
}
