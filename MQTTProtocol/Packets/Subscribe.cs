using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Subscribe : PacketWithPacketId
    {
        public List<Subscription> subList = new List<Subscription>();

        public Subscribe(int packetId, params string[] topics)
        {
            PacketType = (int)MQTTPacketTypes.SUBSCRIBE;
            List<byte> tempPacket = new List<byte>();

            byte lsbOfPacketId = getLsb(packetId);
            byte msbOfPacketId = getMsb(packetId);
            tempPacket.Add(msbOfPacketId);
            tempPacket.Add(lsbOfPacketId);

            foreach (var item in topics)
            {
                byte msbOfTopicLength = getMsb(item.Length);
                byte lsbOfTopicLength = getLsb(item.Length);
                tempPacket.Add(msbOfTopicLength);
                tempPacket.Add(lsbOfTopicLength);
                foreach (char c in item)
                {
                    tempPacket.Add(getByte(c));
                }
                tempPacket.Add(0x00); // quality of service for topic
            }
            CompletePacket(tempPacket);
        }

        public Subscribe(List<byte> packetList)
        {
            subList = new List<Subscription>();
            packetList.RemoveAt(0);
            MemoryStream stream = new MemoryStream(packetList.ToArray());
            int remainingPacketLength = decodeRemainingLength(ref stream);
            PacketId = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());

            while (remainingPacketLength > 0)
            {
                int thisTopicNameLength = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());
                remainingPacketLength -= 2;
                string thisTopicName = string.Empty;
                for (var i = 0; i < thisTopicNameLength; i++)
                {
                    char c = (char)stream.ReadByte();
                    if (c != '\0')
                    {
                        if (_validChars.Contains(c))
                            thisTopicName += c;
                        else
                            return;
                    }
                    remainingPacketLength--;
                }
                int qos = stream.ReadByte();
                subList.Add(new Subscription(thisTopicName, qos));
                remainingPacketLength--;
            }
        }
    }
}
