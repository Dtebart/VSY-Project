using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
{
    public class LoginHandler : RequestHandler
    {
        public override Packet HandleRequest(Packet request, ServerLink serverLink)
        {
            ServerLink.AddClient(request.SrcUser, serverLink.Client);
            return request;
        }
    }
}
