﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetworkLib.RequestHandler
{
    public class FriendlistHandler : RequestHandler
    {
        public override Packet[] HandleRequest(Packet request, ServerLink serverLink)
        {
            UserDBApp dbApp = new UserDBApp("Data Source=DANIEL-PC\\SQLEXPRESS;", "Initial Catalog=UserDB;");
            List<String> friendList = dbApp.GetFriends(request.DestUser);
            dbApp.Close();

            for (int i = 0; i < friendList.Count; i++){
                request.AddParam(friendList[i]);
            }

            Packet[] response = new Packet[1];

            response[0] = request;

            return response;
        }
    }
}
