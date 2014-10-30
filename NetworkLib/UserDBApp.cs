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
            String strSQL = "SELECT Friend, isOnline FROM [User] " +
                            "INNER JOIN [User_User] ON Friend = UserName WHERE [User] = '" + userName + "';";
            SqlCommand cmd = new SqlCommand(strSQL, _con);

            SqlDataReader reader = cmd.ExecuteReader();

            List<String> friends = new List<String>();
            while (reader.Read())
            {
                String friend = reader["Friend"].ToString();
                String onlineStatus = reader["isOnline"].ToString();

                friends.Add(friend);
                friends.Add(onlineStatus);
            }
            reader.Close();
            return friends;
        }

        public List<String> GetOnlineFriends(string userName)
        {
            List<String> onlineFriends = new List<String>();
            String strSQL = "SELECT Friend FROM [User] " +
                            "INNER JOIN [User_User] ON Friend = UserName WHERE [User] = '" + userName + 
                            "' AND isOnline = '1';";
            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                onlineFriends.Add(reader["Friend"].ToString());
            }

            reader.Close();

            return onlineFriends;
        }

        public void InsertFriendship(string user, string friend)
        {
            String strSQL = "INSERT INTO [User_User] VALUES('" + user + "', '" + friend + "');";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Close();
        }

        public void InsertUser(String name, String password)
        {
            String strSQL = "INSERT INTO [User] VALUES('" + name + "', '" + password + "'" + ", '0'" + ");";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Close();
        }

        public void ChangeOnlinestatus(string userName, bool isOnline)
        {
            String strSQL = "UPDATE [User] SET isOnline = '" + Convert.ToInt32(isOnline) + "' WHERE UserName = '" + userName + "';";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Close();
        }

        public bool UserIsOnline(string userName)
        {
            String strSQL = "SELECT isOnline FROM [User] WHERE UserName = '" + userName + "';";

            SqlCommand cmd = new SqlCommand(strSQL, _con);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            bool isOnline = Convert.ToBoolean(reader["isOnline"].ToString());

            reader.Close();

            return isOnline;
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
                        reader.Close();
                        return true;
                    }
                }
            }
            reader.Close();
            return false;
        }

        public void Close(){
            _con.Close();
        }
    }
}
