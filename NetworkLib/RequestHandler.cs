using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public abstract class RequestHandler
    {
        public static RequestHandler GetHandler(MessageTypes type){
            if (type == MessageTypes.TextMessage)
            {
                return new MessageRequestHandler();
            }
            else if (type == MessageTypes.GetFriendlist)
            {
                return new FriendlistHandler();
            }

            return null;
        }

        public abstract Packet HandleRequest(Packet request);
    }
}
