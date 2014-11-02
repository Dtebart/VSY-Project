using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkLib;

namespace NetworkLib.RequestHandler
{
    public class LoginHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            string feedback;
            UserDBApp dbApp = new UserDBApp("Data Source=(local);", "Initial Catalog=UserDB;");
            string userName = request.SrcUser;
            string password = request.AdditionalArgs[0];
            if (dbApp.UserExists(userName, password))
            {
                feedback = "OK";
                ServerLink.AddClient(request.SrcUser, serverLink.Client);
                serverLink._clientName = userName;
                dbApp.ChangeOnlinestatus(userName, true);
            }
            else
            {
                throw new InvalidUserException(serverLink.Client);
            }
            Packet loginFeedback = new Packet(request.SrcUser, feedback, MessageTypes.Login);

            List<String> friendsOfRequester = dbApp.GetOnlineFriends(userName);

            Packet[] response = new Packet[friendsOfRequester.Count + 1];
            response[0] = loginFeedback;

            for (int i = 1; i < response.Length; i++)
            {
                response[i] = new Packet(request.SrcUser, friendsOfRequester[i - 1], "NowOnline", MessageTypes.FriendOnline);
            }

            return response;
        }
    }
}
