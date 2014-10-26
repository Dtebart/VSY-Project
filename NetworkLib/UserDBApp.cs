using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NetworkLib
{
    public class UserDBApp
    {
        private SqlConnection _con;
        public UserDBApp(String serverName, String databaseName)
        {
            _con = new SqlConnection();
            _con.ConnectionString = serverName +
                                    databaseName +
                                    "Integrated Security=True";
            _con.Open();
        }

        public List<String> GetFriends(String userName)
        {
            String strSQL = "SELECT Freund1, Freund2 FROM user_tbl WHERE UserName = '" + userName + "';";
            SqlCommand cmd = new SqlCommand(strSQL, _con);

            SqlDataReader reader = cmd.ExecuteReader();

            List<String> friends = new List<String>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    friends.Add(reader[i].ToString());
                }
            }

            return friends;
        }

        public void Close(){
            _con.Close();
        }
    }
}
