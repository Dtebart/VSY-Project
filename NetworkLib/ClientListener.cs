using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NetworkLib
{
    public class ClientListener
    {
  // ---------------------- Properties ----------------------
        private TcpListener _serverSocket;
        private int _port;
        private NetworkManager _manager;

        // ---------------------- Constructors ----------------------
        public ClientListener(NetworkManager manager)
        {
            _port = 13000;

            _manager = manager;
        }

        public ClientListener(int port)
        {
            _port = port;
            _serverSocket = new TcpListener(IPAddress.Any, _port);
            _serverSocket.Start();
        }

        // ---------------------- Getter/Setter ----------------------
        public TcpListener Listener
        {
            get { return _serverSocket; }
        }

        // ---------------------- Functions ----------------------
        public Packet ReadChannel()
        {
            int i;
            Byte[] dataBuffer = new Byte[256];
            Packet receipt = new Packet();

            while ((i = _manager._stream.Read(dataBuffer, 0, dataBuffer.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                string message = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, i);
                Console.WriteLine("Received: {0}", message);

                receipt._content = message;
                return receipt;
            }

            return receipt;
        }

       /* public void WriteMessage(String message, IPAddress destIp)
        {
            
            Packet packet = new Packet(IPAddress.Loopback, message);
            _stream.Write(packet.Bytes, 0, packet.Bytes.Length);
            Console.WriteLine("Sent: {0}", message);
        }*/

        public Packet Listen()
        {
            _serverSocket = new TcpListener(IPAddress.Loopback, _port);
            _serverSocket.Start();
            TcpClient _server = _serverSocket.AcceptTcpClient();
            Monitor.Enter(_manager._stream);
            _manager._stream = _server.GetStream();
            Packet receipt = ReadChannel();
            _manager._stream.Close();
            Monitor.Exit(_manager._stream);
            return receipt; 
        }
    }
    
}
