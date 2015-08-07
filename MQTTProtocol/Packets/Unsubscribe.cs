using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Unsubscribe : PacketWithPacketId
    {
        public List<string> TopicNames { get; private set; }

        public Unsubscribe(int packetId, params string[] topicNames)
        {
            PacketId = packetId;
            TopicNames = topicNames.ToList();

            PacketType = (int) MQTTPacketTypes.UNSUBSCRIBE;

            List<byte> tempPacket = new List<byte>();
            tempPacket.Add(getMsb(packetId));
            tempPacket.Add(getLsb(packetId));
            foreach (var name in topicNames)
            {
                tempPacket.Add(getMsb(name.Length));
                tempPacket.Add(getLsb(name.Length));
                tempPacket.AddRange(name.Select(getByte));
            }
            CompletePacket(tempPacket);
        }

        public Unsubscribe(List<byte> packetList)
        {
            TopicNames = new List<string>();
            packetList.RemoveAt(0);
            MemoryStream stream = new MemoryStream(packetList.ToArray());
            int remainingPacketLength = decodeRemainingLength(ref stream);
            PacketId = fromMsbLsb((byte)stream.ReadByte(), (byte)stream.ReadByte());

            while (remainingPacketLength > 0)
            {
                int thisTopicNameLength = fromMsbLsb((byte)stream.ReadByte(), (byte)stream.ReadByte());
                remainingPacketLength -= 2;
                string thisTopicName = string.Empty;
                for (var i = 0; i < thisTopicNameLength; i++)
                {
                    char c = (char)stream.ReadByte();
                    if(_validChars.Contains(c))
                        thisTopicName += c;
                    else
                        return;
                        
                    remainingPacketLength--;
                }
                TopicNames.Add(thisTopicName);
            }
        }
    }
}
