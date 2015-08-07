using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace MQTTProtocol.Packets
{
    public abstract class Packet : MQTTBase
    {
        public byte PacketType { get; set; }
        public byte[] FixedHeader { get; set; }
        public List<byte> VariableHeader { get; set; }
        public byte[] PacketBytes { get; set; }
        public string clientID { get; set; }

        protected byte getByte(Char ch)
        {
            return Convert.ToByte(ch);
        }

        protected byte getLsb(int number)
        {
            return (byte)(number & 0xFFu);
        }

        protected byte getMsb(int number)
        {
            return (byte) ((number >> 8) & 0xFFu);
        }

        protected int fromMsbLsb(byte msb, byte lsb)
        {
            return lsb + msb;
        }

        /// <summary>
        /// Adds the fixed header and remaining length, and assigns the variable for the packet
        /// </summary>
        /// <param name="tempPacket">Rest of packet minus the fixed header</param>
        protected void CompletePacket(List<byte> tempPacket)
        {
            if (tempPacket == null) // used for pingreq and other packets with no body
            {
                FixedHeader = new byte[] { PacketType, 0x0 };
                PacketBytes = FixedHeader.ToArray();
            }
            else
            {
                var FixedHeaderList = new List<byte>
                {
                    PacketType
                };

                List<byte> value = encodeRemainingLength(tempPacket.Count);

                FixedHeaderList.AddRange(value);

                List<byte> payloadList = FixedHeaderList;
                payloadList.AddRange(tempPacket);
                PacketBytes = payloadList.ToArray();
            }
        }

        protected List<byte> encodeRemainingLength(int remainingLength)
        {
            List<byte> byteList = new List<byte>();
            do
            {
                int encodedByte = remainingLength % 128;
                remainingLength /= 128;
                if (remainingLength > 0)
                    encodedByte = encodedByte | 128;
                byteList.Add((byte)encodedByte);
            } while (remainingLength > 0);
            return byteList;
        }

        protected int decodeRemainingLength(ref MemoryStream stream)
        {
            int multiplier = 1;
            int value = 0;
            byte nextByte;
            do
            {
                nextByte = (byte)stream.ReadByte();
                value += ((nextByte & 127) * multiplier);
                multiplier *= 128;
            } while ((nextByte & 128) != 0);
            return value;
        }
    }
}
