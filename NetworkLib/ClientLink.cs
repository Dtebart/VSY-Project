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
        public IClient _iClient;
        private Thread _readThread;
        private IPAddress[] _IPserver;

        // ---------------------- Constructors ----------------------

        // Server is the same as the client; only for debugging purposes
        public ClientLink(IClient client)
        {
            _port = 13000;
            _IPserver = new IPAddress[2];
            _IPserver[1] = IPAddress.Parse("192.168.220.104");
            _IPserver[0] = IPAddress.Parse("192.168.220.112");
            Connect();
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
            _readThread.SetApartmentState(ApartmentState.STA);

            _readThread.Start();
        }

        public void Serve()
        {
            while (_client.Connected)
            {
                try
                {
                    Packet recievedPacket = ReadChannel();
                    _iClient.ActionAfterRead(recievedPacket);
                }
                catch(System.IO.IOException e)
                {
                    _iClient.Disconnected();
                }
                
            }
        }

        public void Close()
        {
            _readThread.Abort();
            _client.Close();
        }

        public static IPAddress GetLocalIP()
        {
            IPHostEntry host;
            String localIP = String.Empty;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return IPAddress.Parse(localIP);
        }


        public void Connect(int array = 0)
        {
            IPAddress IPclient = GetLocalIP();
            try
            {
                IPEndPoint serverEndPoint = new IPEndPoint(_IPserver[array], 13000);
                IPEndPoint clientEndPoint = new IPEndPoint(IPclient, 0);
                _client = new TcpClient(clientEndPoint);
                _client.Connect(serverEndPoint);
            }
            catch(SocketException e)
            {
                if (_IPserver.Length > array)
                Connect(array + 1);
            }
        }
    }
}
