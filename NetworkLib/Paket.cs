using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public enum MessageTypes { TextMessage, Login, Registrate, GetFriendlist };
    public class Packet
    {
        // ---------------------- Properties ----------------------
        private string _srcUser;
        private string _destUser;
        internal string _content;
        private MessageTypes _messageType;
        private List<String> _additionalArgs = new List<String>();
        private const char _splitToken = '\t';

        // ---------------------- Constructors ----------------------
      
        public Packet()
        {
            _content = String.Empty;
        }

        public Packet(string message)
        {
            String[] values = message.Split(_splitToken);
            _messageType = (MessageTypes) Int32.Parse(values[0]);
            _destUser = values[1];
            _srcUser = values[2];
           
            for (int i = 3; i < values.Length - 1; i++)
            {
                AddParam(values[i]);
            }

            _content = values[values.Length - 1];
        }

        public Packet(string destUser, string content, MessageTypes messageType)
        {
            _destUser = destUser;
            _content = content;
            _messageType = messageType;
            _srcUser = "NOTSET";
        }

        public Packet(string srcUser, string destUser, string content, MessageTypes messageType)
            : this(destUser, content, messageType)
        {
            _srcUser = srcUser;
        }

        // ---------------------- Getter/Setter ----------------------
        public string DestUser
        {
            get { return _destUser; }
        }
        public string Content
        {
            get { return _content; }
        }
        public MessageTypes Type
        {
            get { return _messageType; }
        }

        public string SrcUser
        {
            get { return _srcUser; }
        }

        public String AdditionalArgsText
        {
            get {
                String argContent = String.Empty;
                for (int i = 0; i < _additionalArgs.Count; i++)
                {
                    argContent += _additionalArgs[i].ToString() + _splitToken;
                }
                return argContent;
            }
        }

        public List<String> AdditionalArgs
        {
            get { return _additionalArgs; }
        }

        public Byte[] Bytes
        {
            get {
                String messageText = ((int)_messageType).ToString() + _splitToken + _destUser + _splitToken + _srcUser + _splitToken + AdditionalArgsText + _content;
                return System.Text.Encoding.ASCII.GetBytes(messageText); 
            }
        }

        // ---------------------- Functions ----------------------

        public void AddParam(String param)
        {
            _additionalArgs.Add(param);
        }
    }
}
