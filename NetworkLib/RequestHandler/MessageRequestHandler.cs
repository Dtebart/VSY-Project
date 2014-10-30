using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
{
    public class MessageRequestHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            Packet[] response = new Packet[1];

            response[0] = request;

            return response;
        }
    }
}
