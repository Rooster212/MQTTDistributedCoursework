using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MQTTProtocol.Packets
{
    public class Connack : Packet
    {
        public Connack()
        {
            PacketType = (int) MQTTPacketTypes.CONNACK;
            List<byte> tempPacket = new List<byte>();
            tempPacket.Add(0x00); // connect ack flags
            tempPacket.Add(0x00); // return code

            CompletePacket(tempPacket);
        }

        public Connack(StreamReader reader)
        {
             
        }
    }
}
