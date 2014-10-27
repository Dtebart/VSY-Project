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
        private static Dictionary<string, TcpClient> _clientList;
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

        public ServerLink(TcpListener serverSocket, Dictionary<string, TcpClient> clientList)
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

        public TcpClient Client
        {
            get { return _client; }
        }
        public Dictionary<string, TcpClient> ClientList
        {
            get { return _clientList; }
        }

        // ---------------------- Functions ----------------------

        public TcpClient Listen()
        {
            _serverSocket.Start();
            _client = _serverSocket.AcceptTcpClient();
            _stream = _client.GetStream();

            IPEndPoint clientPoint = (IPEndPoint)_client.Client.RemoteEndPoint;
            IPAddress srcIp = clientPoint.Address;

            return _client;
        }

        public static void AddClient(String clientName, TcpClient client)
        {
            Monitor.Enter(_clientList);
                _clientList.Add(clientName, client);
            Monitor.Exit(_clientList);
        }

        public bool Open()
        {
            return _client.Connected;
        }
    }
}
