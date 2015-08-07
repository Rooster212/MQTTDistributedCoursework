using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace MQTTProtocol
{
    public enum MQTTPacketTypes : byte
    {
        CONNECT = 0x10,
        CONNACK = 0x20,
        PUBLISH = 0x30,
        PUBACK = 0x40,
        PUBREC = 0x50,
        PUBREL = 0x60,
        PUBCOMP = 0x70,
        SUBSCRIBE = 0x82,
        SUBACK = 0x90,
        UNSUBSCRIBE = 0xa2,
        UNSUBACK = 0xb0,
        PINGREQ = 0xc0,
        PINGRESP = 0xd0,
        DISCONNECT = 0xe0
    }

    public class MQTTBase
    {
        public char[] _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/\\\"*:;@#[]()£$%^&*'~{}".ToCharArray();

        internal const byte MQTT_PROTOCOL_V3_1_1 = 0x04;
        internal const byte CONNECT_FLAGS = 0x00;
        internal const int KEEP_ALIVE_TIME = 60;
        public const int TCP_TIMEOUT = 2000;
        public const int BUFFER_SIZE = 2048;
        public const int THREAD_SLEEP_REST_TIME = 5;
        public const int PING_INTERVAL_MS = 30000;
    }
}
