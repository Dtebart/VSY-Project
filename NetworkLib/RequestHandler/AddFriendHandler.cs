using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetworkLib.RequestHandler
{
    public class AddFriendHandler : RequestHandler
    {

        UserDBApp _DB;
        public AddFriendHandler (UserDBApp UserDB)
        {
            _DB = UserDB;
        }

        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            //UserDBApp dbApp = new UserDBApp("Data Source=(local);", "Initial Catalog=UserDB;");

            String newFriend = request.Content.Replace("\n", "");
            Packet[] response = new Packet[1];
            try
            {
                _DB.InsertFriendship(request.SrcUser, newFriend);
                request.AddParam(_DB.UserIsOnline(newFriend).ToString());

                response[0] = request;
            }
            catch (SqlException e)
            {
                Console.WriteLine("User {0} requested from {1} not found!\n", newFriend, request.SrcUser);
                Packet responsePacket = new Packet(request.SrcUser, "ERROR", request.Type);

                response[0] = responsePacket;
            }

            return response;
        }
    }
}
