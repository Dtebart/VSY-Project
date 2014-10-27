using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NetworkLib
{
    public class InvalidUserException : Exception 
    {
        public TcpClient _client;
        public InvalidUserException(TcpClient client) {
            _client = client;
        }
        public InvalidUserException(string message) : base(message) { }
        public InvalidUserException(string message, Exception inner) : base(message, inner) { }
    }
}
