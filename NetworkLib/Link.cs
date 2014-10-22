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
    public abstract class Link
    {
        // ---------------------- Properties ----------------------
        protected int _port;
        protected TcpClient _client;
        protected NetworkStream _stream;

        // ---------------------- Functions ----------------------
        public Packet ReadChannel()
        {
            int i;
            Byte[] dataBuffer = new Byte[256];
            Packet receipt = new Packet();

            do
            {
                i = _stream.Read(dataBuffer, 0, dataBuffer.Length);
                // Translate data bytes to a ASCII string.
                string message = System.Text.Encoding.ASCII.GetString(dataBuffer, 0, i);
                Console.WriteLine("Received: {0}", message);

                receipt._content += message;
            } while (receipt.Content[receipt.Content.Length - 1] != '\n');
            receipt = new Packet(receipt._content);
            return receipt;
        }

        public void WriteMessage(String message, IPAddress destIp)
        {
            if (message[message.Length - 1] != '\n')
                message += '\n';
            Packet packet = new Packet(destIp, message);
            // Send the message to the connected TcpServer. 
            _stream.Write(packet.Bytes, 0, packet.Bytes.Length);
        }

        public void WriteMessage(Packet message)
        {
            if (message._content[message._content.Length - 1] != '\n')
                message._content += '\n';

            _stream.Write(message.Bytes, 0, message.Bytes.Length);
        }

        public void WriteMessage(Packet message, NetworkStream destStream)
        {
            if (message._content[message._content.Length - 1] != '\n')
                message._content += '\n';

            destStream.Write(message.Bytes, 0, message.Bytes.Length);
        }
    }
}

