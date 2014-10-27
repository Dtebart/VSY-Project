using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
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
            else if (type == MessageTypes.Login)
            {
                return new LoginHandler();
            }

            return null;
        }

        public abstract Packet HandleRequest(Packet request, ServerLink serverLink);
    }
}
