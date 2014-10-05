using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace NetworkLib
{
    public class MessageTokenizer
    {
        static char[] _keychars = { '\t' };
        public MessageTokenizer()
        {
            Packet test = CreatePacket("5\t192.168.0.1\tDaniel dasd asdasd a dsasd aaa");
        }

        public MessageTokenizer(char[] keys)
        {
            _keychars = keys;
        }


        public static Packet CreatePacket(string _string)
        {

            String[] values = _string.Split(_keychars);
            Packet reconstructedPacket = new Packet(IPAddress.Parse(values[1]), values[2], values[0]);
            return reconstructedPacket;
        }

        public static Packet CreatePacket(string _string, char[] keys)
        {
            String[] values = _string.Split(keys);
            Packet reconstructedPacket = new Packet(IPAddress.Parse(values[1]), values[2], values[0]);
            return reconstructedPacket;
        }


    }
}
