using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAMB
{
    public static class UserSessions
    {
        public static User CurrentUser
        {
            get => Application.Current.Properties["CurrentUser"] as User;
            set => Application.Current.Properties["CurrentUser"] = value;
        }

        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsLibrarian => CurrentUser?.IsLibrarian() ?? false;
        public static bool IsGuest => CurrentUser?.IsGuest() ?? false;
    }
}
