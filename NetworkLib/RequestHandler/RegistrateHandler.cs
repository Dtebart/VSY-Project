using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetworkLib.RequestHandler
{
    public class RegistrateHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            String user = request.SrcUser;
            String password = request.AdditionalArgs[0];
            Packet[] response = new Packet[1];
            try
            {
                UserDBApp dbApp = new UserDBApp("Data Source=(local);", "Initial Catalog=UserDB;");
                dbApp.InsertUser(user, password);

                ServerLink.AddClient(request.SrcUser, serverLink.Client);

                response[0] = request;
                serverLink._clientName = request.SrcUser;
            }
            catch (SqlException e)
            {
                throw new InvalidUserException(serverLink.Client);
            }

            return response;
        }
    }
}
