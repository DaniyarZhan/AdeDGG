using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAMB
{
    public class UserManager
    {
        private List<User> users = new List<User>();
        private string saveFileName = "users.json";

        public List<User> Users => users;

        public void AddUser(User user)
        {
            if (user != null && !users.Any(u => u.Login == user.Login))
            {
                users.Add(user);
            }
        }

        public void RemoveUser(User user)
        {
            if (users.Contains(user))
                users.Remove(user);
        }

        public User FindUser(string login, string password)
        {
            return users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var data = JsonSerializer.Serialize(users, options);
            File.WriteAllText(saveFileName, data);
        }

        public void Load()
        {
            if (!File.Exists(saveFileName)) return;

            var data = File.ReadAllText(saveFileName);
            if (!string.IsNullOrEmpty(data))
                users = JsonSerializer.Deserialize<List<User>>(data);
            else
                users = new List<User>();
        }
    }
}
