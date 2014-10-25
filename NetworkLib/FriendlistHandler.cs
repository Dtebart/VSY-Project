using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class FriendlistHandler : RequestHandler
    {
        public override Packet HandleRequest(Packet request)
        {
            request.AddParam("178.201.225.83");
            request.AddParam("77.182.137.206");
            return request;
        }
    }
}
