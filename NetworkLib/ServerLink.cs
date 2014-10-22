using System;
using System.Threading;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class ServerLink : Link
    {
        // ---------------------- Properties ----------------------
        private TcpListener _serverSocket;

        // ---------------------- Constructors ----------------------
        public ServerLink()
        {
            _port = 13000;
            _serverSocket = new TcpListener(IPAddress.Any, _port);
            _serverSocket.Start();
        }

        public ServerLink(int port)
        {
            _port = port;
            _serverSocket = new TcpListener(IPAddress.Any, _port);
            _serverSocket.Start();
        }

        public ServerLink(TcpListener serverSocket)
        {
            _port = 13000;
            _serverSocket = serverSocket;
        }

        // ---------------------- Getter/Setter ----------------------
        public TcpListener Listener
        {
            get { return _serverSocket; }
        }

        public TcpClient Client
        {
            get { return _client; }
        }

        // ---------------------- Functions ----------------------

        public TcpClient Listen()
        {
            _serverSocket.Start();
            _client = _serverSocket.AcceptTcpClient();
            _stream = _client.GetStream();
            return _client;
        }

        public bool Open()
        {
            return _client.Connected;
        }
    }
}
