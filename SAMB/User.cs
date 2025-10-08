using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMB
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Библиотекарь", "Гость"

        public User() { }

        public User(string login, string password, string role)
        {
            Login = login;
            Password = password;
            Role = role;
        }
        // Методы для проверки прав
        public bool IsLibrarian() => Role == "Библиотекарь";
        public bool IsGuest() => Role == "Гость";
        public bool CanEditBooks() => IsLibrarian();
        public bool CanManageUsers() => IsLibrarian();
        
    }
}
