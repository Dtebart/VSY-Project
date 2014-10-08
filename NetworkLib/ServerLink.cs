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
        private ArrayList _clientList;

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

        public ServerLink(TcpListener serverSocket, ArrayList clientList)
        {
            _port = 13000;
            _serverSocket = serverSocket;
            _clientList = clientList;
        }

        // ---------------------- Getter/Setter ----------------------
        public TcpListener Listener
        {
            get { return _serverSocket; }
        }

        // ---------------------- Functions ----------------------

 public TcpClient Listen()
{
_serverSocket.Start();
_client = _serverSocket.AcceptTcpClient();
_stream = _client.GetStream();
Monitor.Enter(_clientList);
_clientList.Add(_client);
Monitor.Exit(_clientList);
return _client;
}



        public bool Open()
        {
            return _client.Connected;
        }
    }
}
