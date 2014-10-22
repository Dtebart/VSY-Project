using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class ClientLink : Link
    {
        // ---------------------- Properties ----------------------
        private IClient _iClient;
        private Thread _readThread;

        // ---------------------- Constructors ----------------------

        // Server is the same as the client; only for debugging purposes
        public ClientLink(IClient client)
        {
            _port = 13000;
            IPAddress _IPserver = IPAddress.Parse("178.201.225.83");
            IPAddress _IPclient = IPAddress.Parse("192.168.0.101");
            IPEndPoint _serverEndPoint = new IPEndPoint(_IPserver, 13000);
            IPEndPoint _clientEndPoint = new IPEndPoint(_IPclient, 0);
            _client = new TcpClient(_clientEndPoint);
            _client.Connect(_serverEndPoint);
            _iClient = client;
            
            StartReading();
        }

        public ClientLink(IClient client, int port)
        {
            _port = port;
            _iClient = client;
            StartReading();
        }

        // ---------------------- Functions ----------------------
        private void StartReading()
        {
            _stream = _client.GetStream();

            // Start thread for reading data
            ThreadStart readDel = new ThreadStart(Serve);
            _readThread = new Thread(readDel);

            _readThread.Start();
        }

        public void Serve()
        {
            while (_client.Connected)
            {
                Packet recievedPacket = ReadChannel();
                _iClient.ActionAfterRead(recievedPacket);
            }
        }

        public void Close()
        {
            // get rid of read thread
            _readThread.Abort();
        }
    }
}
