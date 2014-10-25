using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class MessageRequestHandler : RequestHandler
    {
        public override Packet HandleRequest(Packet request)
        {
            return request;
        }
    }
}
