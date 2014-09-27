using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class Paket
    {
        private IPAddress _srcIp;
        private IPAddress _destIp;
        private string _content;

        // ---------------------- Constructors ----------------------
        public Paket()
        {
            _srcIp = IPAddress.Loopback;
            _destIp = IPAddress.Loopback;
            _content = "Hello World!";
        }

        public Paket(IPAddress srcIp, IPAddress destIp){
            _srcIp = srcIp;
            _destIp = destIp;
            _content = String.Empty;
        }

        public Paket(IPAddress srcIp, IPAddress destIp, string content)
            : this(srcIp, destIp)
        {
            _content = content;
        }

        // ---------------------- Getter/Setter ----------------------
        public IPAddress SrcIp
        {
            get { return _srcIp; }
        }
        public IPAddress DestIp
        {
            get { return _destIp; }
        }
        public string Content
        {
            get { return _content; }
        }
        public Byte[] Bytes
        {
            get { return System.Text.Encoding.ASCII.GetBytes(_content); }
        }
    }
}
