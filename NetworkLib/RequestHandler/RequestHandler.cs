using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace NetworkLib.RequestHandler
{

    public abstract class RequestHandler
    {
        static UserDBApp dbApp = null;

        private static Dictionary<MessageTypes, RequestHandler> _requestHandlerDict = new Dictionary<MessageTypes, RequestHandler>();

        public static void RegistrateHandler()
        {
            RequestHandler._requestHandlerDict.Add(MessageTypes.AddFriend, new AddFriendHandler(dbApp));
            RequestHandler._requestHandlerDict.Add(MessageTypes.GetFriendlist, new FriendlistHandler(dbApp));
            RequestHandler._requestHandlerDict.Add(MessageTypes.Login, new LoginHandler(dbApp));
            RequestHandler._requestHandlerDict.Add(MessageTypes.Registrate, new RegistrateHandler(dbApp));
            RequestHandler._requestHandlerDict.Add(MessageTypes.TextMessage, new MessageRequestHandler());
        }
        public static void RegistrateDataBase()
        {
            if (dbApp == null)
            {
                try
                {
                    dbApp = new UserDBApp("Data Source=DANIELT-PC\\SQLEXPRESS;", "Initial Catalog=master;");

                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    dbApp = new UserDBApp("Data Source=(local);", "Initial Catalog=master;");
                }
            }
        }

        public static RequestHandler GetHandler(MessageTypes type){
            return RequestHandler._requestHandlerDict[type];
        }

        public abstract Packet[] HandleRequest(Packet request, ServerLink serverLink);
    }
}
