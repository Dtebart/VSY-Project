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
        public static RequestHandler GetHandler(MessageTypes type){

            if (dbApp == null)
            {
                try
                {
                    dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;"); 
    
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    dbApp = new UserDBApp("Data Source=(local);", "Initial Catalog=UserDB;");    
                }
            }

            if (type == MessageTypes.TextMessage)
            {
                return new MessageRequestHandler();
            }
            else if (type == MessageTypes.GetFriendlist)
            {
                return new FriendlistHandler(dbApp);
            }
            else if (type == MessageTypes.Login)
            {
                return new LoginHandler(dbApp);
            }
            else if (type == MessageTypes.Registrate)
            {
                return new RegistrateHandler(dbApp);
            }
            else if (type == MessageTypes.AddFriend)
            {
                return new AddFriendHandler(dbApp);
            }

            return null;
        }

        public abstract Packet[] HandleRequest(Packet request, ServerLink serverLink);
    }
}
