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
    public class ClientLink : ILink
    {
        // ---------------------- Properties ----------------------
        private int _port;
        private string _server;
        private Packet _lastPacket;
        private TcpClient _clientSocket;
        private NetworkStream _stream;
        private IClient _client;
        private Thread _readThread;

        // ---------------------- Constructors ----------------------

        // Server is the same as the client; only for debugging purposes
        public ClientLink(IClient client)
        {
            _port = 13000;
            _server = System.Environment.MachineName;
            _clientSocket = new TcpClient(_server, _port);
            _client = client;
            StartReading();
        }

        public ClientLink(IClient client, int port, string server)
        {
            _port = port;
            _server = server;
            _clientSocket = new TcpClient(_server, _port);
            _client = client;
            StartReading();
        }

        // ---------------------- Functions ----------------------
        private void StartReading()
        {
            _stream = _clientSocket.GetStream();

            // Start thread for reading data
            ThreadStart readDel = new ThreadStart(ReadChannel);
            _readThread = new Thread(readDel);

            _readThread.Start();
        }

        public void ReadChannel()
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            int i;
            while (true)
            {
                _lastPacket = new Packet();
                while ((i = _stream.Read(data, 0, data.Length)) != 0)
                {
                    _lastPacket._content = System.Text.Encoding.ASCII.GetString(data, 0, i);
                    _client.ActionAfterRead(_lastPacket);
                }
            }
        }

        public void WriteMessage(String message, IPAddress destIp)
        {
            Packet packet = new Packet(destIp, message);

            // Send the message to the connected TcpServer. 
            _stream.Write(packet.Bytes, 0, packet.Bytes.Length);
        }

        public void Close()
        {
            // get rid of read thread
            _readThread.Abort();
        }
    }
}
