using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class Packet
    {
        // ---------------------- Properties ----------------------
        private string _destUser;
        internal string _content;
        private string _messageType;
        private char _split = '\t';

        // ---------------------- Constructors ----------------------
      
        public Packet()
        {
            //_destIp = IPAddress.Loopback;
            _content = String.Empty;
        }

        public Packet(string message)
        {
            char[] split = {'\t'};
            String[] values = message.Split(split);
            _messageType = values[0];
            _destUser = values[1];
            _content = values[2];
        }


        public Packet(string destUser, string content)
        {
            _destUser = destUser;
            _content = content;
            IPHostEntry ipHost = Dns.GetHostEntry(System.Environment.MachineName);
            _messageType = "1";
        }

        public Packet(string destUser, string content, string messageType)
        {
            _destUser = destUser;
            _content = content;
            IPHostEntry ipHost = Dns.GetHostEntry(System.Environment.MachineName);
            _messageType = messageType;
        }



        // ---------------------- Getter/Setter ----------------------
        public string DestUser
        {
            get { return _destUser; }
        }
        public string Content
        {
            get { return _content; }
        }
        public Byte[] Bytes
        {
            get { return System.Text.Encoding.ASCII.GetBytes(_messageType + _split + _destUser + _split + _content); }
        }

        // ---------------------- Functions ----------------------
    }
}
