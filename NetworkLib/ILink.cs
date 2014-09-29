using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public interface ILink
    {
        void ReadChannel();
        void WriteMessage(String message, IPAddress destIp);
    }
}
