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
            String strSQL = "SELECT UserName1, UserName2 FROM User_User " +
                            "WHERE UserName1 = '" + userName + "' OR UserName2 = '" + userName + "';";
            SqlCommand cmd = new SqlCommand(strSQL, _con);

            SqlDataReader reader = cmd.ExecuteReader();

            List<String> friends = new List<String>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader[i].ToString();
                    if (name != userName){
                        friends.Add(name);
                    }
                }
            }
            return friends;
        }

        public void InsertUser(String name, String password)
        {
            String strSQL = "INSERT INTO [User] VALUES('" + name + "', '" + password + "');";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            cmd.ExecuteReader();
        }

        public bool UserExists(string userName, string password)
        {
            String strSQL = "SELECT Password FROM [User] WHERE UserName = '" + userName + "';";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader[i].ToString().Equals(password))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Close(){
            _con.Close();
        }
    }
}
