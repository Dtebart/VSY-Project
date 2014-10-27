using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
{
    public class AddFriendHandler : RequestHandler
    {
        public override Packet HandleRequest(Packet request, ServerLink serverLink)
        {
            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");

            dbApp.InsertFriendship(request.SrcUser, request.Content.Replace("\n", ""));

            return request;
        }
    }
}
