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
            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");
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

            Packet[] response = new Packet[1];

            response[0] = loginFeedback;

            return response;
        }
    }
}
