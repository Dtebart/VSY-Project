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
        private IPAddress _srcIp;
        private IPAddress _destIp;
        internal string _content;
        private string _messageType;
        private char _split = '\t';
        private static string _globalIP = null;
        // ---------------------- Constructors ----------------------
       
        
        
        public Packet()
        {
            _srcIp = IPAddress.Loopback;
            _destIp = IPAddress.Loopback;
            _content = String.Empty;
        }

        public Packet(string message)
        {
            if (_globalIP == null)
            {
                _globalIP = GetPublicIP();
            }
            char[] split = {'\t'};
            String[] values = message.Split(split);
            _messageType = values[0];
            _destIp = IPAddress.Parse(values[1]);
            _content = values[2];
            _srcIp = IPAddress.Parse(_globalIP);
        }

        public Packet(IPAddress srcIp, IPAddress destIp){
            _srcIp = srcIp;
            _destIp = destIp;
            _content = String.Empty;
        }

        public Packet(IPAddress destIp, string content)
        {

            if (_globalIP == null)
            {
                _globalIP = GetPublicIP();
            }
            _destIp = destIp;
            _content = content;
            IPHostEntry ipHost = Dns.GetHostEntry(System.Environment.MachineName);
            _srcIp = IPAddress.Parse(_globalIP);
            _messageType = "1";
        }

        public Packet(IPAddress destIp, string content, string messageType)
        {
            if (_globalIP == null)
            {
                _globalIP = GetPublicIP();
            }
            _destIp = destIp;
            _content = content;
            IPHostEntry ipHost = Dns.GetHostEntry(System.Environment.MachineName);
            _srcIp = IPAddress.Parse(_globalIP);
            _messageType = messageType;
        }

        public Packet(IPAddress srcIp, IPAddress destIp, string content)
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
            get { return System.Text.Encoding.ASCII.GetBytes(_messageType + _split + _destIp.ToString() + _split + _content); }
        }

        // ---------------------- Functions ----------------------
        private static string GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }
    }
}
