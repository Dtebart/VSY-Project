using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib.RequestHandler
{
    public class RegistrateHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            String user = request.SrcUser;
            String password = request.AdditionalArgs[0];

            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");
            dbApp.InsertUser(user, password);

            ServerLink.AddClient(request.SrcUser, serverLink.Client);

            Packet[] response = new Packet[1];

            response[0] = request;

            return response;
        }
    }
}
