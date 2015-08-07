using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MQTTProtocol.Packets
{
    public class Connect : Packet
    {
        public string ClientId { get; private set; }
        public string ProtocolName { get; private set; }
        public byte ProtocolVersion { get; private set; }
        public byte ConnectFlags { get; private set; }
        public int KeepAliveTime { get; private set; }


        public Connect(string clientId)
        {
            ClientId = clientId;

            PacketType = (int)MQTTPacketTypes.CONNECT;

            // VARIABLE HEADER
            VariableHeader = new List<byte>();

            byte lsbOfKeepAliveTime = getLsb(KEEP_ALIVE_TIME);
            byte msbOfKeepAliveTime= getMsb(KEEP_ALIVE_TIME);
            
            var variableHeader = new byte[]{
                0x00, // protocol name length msb
                0x04, // protocol name length lsb
                getByte('M'), // protocol name
                getByte('Q'),
                getByte('T'),
                getByte('T'), 
                MQTT_PROTOCOL_V3_1_1, // protocol level
                CONNECT_FLAGS, // connect flags
                msbOfKeepAliveTime, // keep alive time MSB
                lsbOfKeepAliveTime // keep alive time LSB (60s)
            };

            // ADD TO TEMP PACKET
            List<byte> tempPacket = variableHeader.ToList();

            byte[] clientIdBytes = Encoding.UTF8.GetBytes(clientId);
            
            // add client ID to temp packet
            int lengthOfClientId = clientIdBytes.Length;
            byte lsbOfClientId = getLsb(lengthOfClientId);
            byte msbOfClientId = getMsb(lengthOfClientId);

            tempPacket.Add(msbOfClientId);
            tempPacket.Add(lsbOfClientId);

            tempPacket.AddRange(clientIdBytes);

            // BUILD FINAL PACKET
            CompletePacket(tempPacket);
        }

        public Connect(List<byte> packetList)
        {
            packetList.RemoveAt(0); // remove packet type
            MemoryStream stream = new MemoryStream(packetList.ToArray());
            int numberOfChars = decodeRemainingLength(ref stream);

            int protocolNameLength = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());

            ProtocolName = string.Empty;
            for (int i = 0; i < protocolNameLength; i++)
            {
                char c = (char)stream.ReadByte();

                if (c != '\0')
                    ProtocolName += c;
            }

            ProtocolVersion = (byte) stream.ReadByte();
            // possibly check for the protocol level

            ConnectFlags = (byte) stream.ReadByte();

            KeepAliveTime = fromMsbLsb((byte) stream.ReadByte(), (byte) stream.ReadByte());

            int clientIdLength = fromMsbLsb((byte)stream.ReadByte(), (byte)stream.ReadByte());

            ClientId = string.Empty;
            for (int i = 0; i < clientIdLength; i++)
            {
                char c = (char)stream.ReadByte();
                if (c != '\0')
                    ClientId += c;
            }

        }
    }
}
