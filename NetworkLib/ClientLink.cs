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
    public class ClientLink
    {
        // ---------------------- Properties ----------------------
        private int _port;
        private string _server;
        private Packet _lastPacket;
        private TcpClient _clientSocket;
        private IClient _client;
        private Thread _readThread;
        private NetworkManager _manager;

        // ---------------------- Constructors ----------------------

        // Server is the same as the client; only for debugging purposes
        public ClientLink(NetworkManager manager)
        {
            _port = 13000;
            _server = System.Environment.MachineName;
            
            _manager = manager;
        }

        public ClientLink(IClient client, int port, string server)
        {
            _port = port;
            _server = server;
            _clientSocket = new TcpClient(_server, _port);
            _client = client;
            
        }

        // ---------------------- Functions ----------------------
       /* private void StartReading()
        {
            

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
        }*/

        public void WriteMessage(String message, IPAddress destIp)
        {
            Monitor.Enter(_manager._stream);
            _clientSocket = new TcpClient(_server, _port);
            _manager._stream = _clientSocket.GetStream();
            
            if (message[message.Length - 1] != '\n')
                message += '\n';

            Packet packet = new Packet(destIp, message);
            
            // Send the message to the connected TcpServer. 
            _manager._stream.Write(packet.Bytes, 0, packet.Bytes.Length);
            _clientSocket.Close();
            _manager._stream.Close();
            Monitor.Exit(_manager._stream);
        }

        public void Close()
        {
            // get rid of read thread
            _readThread.Abort();
        }
    }
}
