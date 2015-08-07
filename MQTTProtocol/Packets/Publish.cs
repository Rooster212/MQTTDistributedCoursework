using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Publish : Packet
    {
        public string TopicName { get; private set; }
        public string Payload { get; private set; }

        public Publish(string topicName, string payload)
        {
            Payload = payload;
            TopicName = topicName;
            PacketType = (int) MQTTPacketTypes.PUBLISH;
            List<byte> tempPacket = new List<byte>();
            // add topic name
            byte[] topicNameBytes = Encoding.UTF8.GetBytes(topicName);
            byte lsbTopicNameLength = getLsb(topicNameBytes.Length);
            byte msbTopicNameLength = getMsb(topicNameBytes.Length);
            tempPacket.Add(msbTopicNameLength);
            tempPacket.Add(lsbTopicNameLength);
            tempPacket.AddRange(topicNameBytes);
            // add payload
            if (!String.IsNullOrEmpty(payload))
            {
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
                tempPacket.AddRange(payloadBytes);
            }
            // complete packet
            CompletePacket(tempPacket);
        }

        public Publish(List<byte> packetList)
        {
            packetList.RemoveAt(0); // remove the type which we know already
            MemoryStream stream = new MemoryStream(packetList.ToArray());
            int numberOfChars = decodeRemainingLength(ref stream);

            int topicNameLength = fromMsbLsb((byte)stream.ReadByte(), (byte)stream.ReadByte());

            TopicName = string.Empty;
            for(int i = 0; i < topicNameLength; i++)
            {
                char c = (char) stream.ReadByte();

                if(c != '\0')
                    TopicName += c;
            }

            int remainingLength = numberOfChars - 2 - topicNameLength; // -2 for topic name length MSB+LSB
            Payload = string.Empty;
            for (int i = 0; i < remainingLength; i++)
            {
                char c = (char) stream.ReadByte();
                if(c != '\0')
                    Payload += c;
            }
            stream.Close();
        }
    }
}
