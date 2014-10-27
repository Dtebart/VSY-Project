using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSY_Client
{
    class ChatHistory
    {
        private Dictionary<string, string> _chatHistory;

       public ChatHistory()
        {
            _chatHistory = new Dictionary<string, string>();
        }

        public void AddUser(string user)
        {
            _chatHistory.Add(user, "");
        }

        public void AddTexttoUser(string text, string user)
        {
            _chatHistory[user] = _chatHistory[user] += text;
        }

        public void DeleteUser(string user)
        {
            _chatHistory.Remove(user);
        }

        public Boolean UserExist(string user)
        {
            return _chatHistory.ContainsKey(user);
        }

        public string GetText(string user)
        {
            return _chatHistory[user];
        }
    }
}
