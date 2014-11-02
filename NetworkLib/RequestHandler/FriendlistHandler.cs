using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetworkLib.RequestHandler
{
    public class FriendlistHandler : RequestHandler
    {
        UserDBApp _DB;

        public FriendlistHandler(UserDBApp UserDB)
        {
            _DB = UserDB;
        }

        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            List<String> friendList = _DB.GetFriends(request.DestUser);

            for (int i = 0; i < friendList.Count; i++){
                request.AddParam(friendList[i]);
            }

            Packet[] response = new Packet[1];

            response[0] = request;

            return response;
        }
    }
}
