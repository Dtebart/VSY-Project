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
            Packet receipt;

            string message = String.Empty;

            do
            {
                i = _stream.Read(dataBuffer, 0, dataBuffer.Length);
                // Translate data bytes to a ASCII string.
                message += System.Text.Encoding.BigEndianUnicode.GetString(dataBuffer, 0, i);


            } while (message[message.Length - 1] != '\n');

            Console.WriteLine("Received: {0}", message);
            receipt = new Packet(message);
            return receipt;
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

