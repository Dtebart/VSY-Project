using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
{
    public class AddFriendHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");

            String newFriend = request.Content.Replace("\n", "");
            dbApp.InsertFriendship(request.SrcUser, newFriend);
            request.AddParam(dbApp.UserIsOnline(newFriend).ToString());

            Packet[] response = new Packet[1];

            response[0] = request;

            return response;
        }
    }
}
