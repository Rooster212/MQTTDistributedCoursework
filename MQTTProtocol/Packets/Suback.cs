using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Suback : PacketWithPacketId
    {
        /// <summary>
        /// List of successful subscription results. True signifies success.
        /// This will need changing if the full specification is implemented
        /// </summary>
        public List<bool> topicSubscriptionResults = new List<bool>();

        public Suback(int packetId, params int[] topicSubscriptionResults)
        {
            PacketType = (int)MQTTPacketTypes.SUBACK;
            List<byte> tempPacket = new List<byte>();

            byte lsbPacketId = getLsb(packetId);
            byte msbPacketId = getMsb(packetId);

            tempPacket.Add(msbPacketId);
            tempPacket.Add(lsbPacketId);
            foreach (var b in topicSubscriptionResults)
            {
                tempPacket.Add((byte)b);
            }
            CompletePacket(tempPacket);
        }

        public Suback(List<byte> packet)
        {
            packet.RemoveAt(0);
            MemoryStream stream = new MemoryStream(packet.ToArray());

            int remainingPacketLength = decodeRemainingLength(ref stream);

            PacketId = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());

            while (remainingPacketLength > 0)
            {
                byte result = (byte)stream.ReadByte();
                if(result == 0x80)
                    topicSubscriptionResults.Add(false);
                else
                    topicSubscriptionResults.Add(true);
                remainingPacketLength--;
            }
        }
    }
}
