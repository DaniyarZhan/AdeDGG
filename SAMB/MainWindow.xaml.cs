using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAMB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string login;
        private string password;
        private readonly string fileName = "user.json";
        private UserManager userManager = new UserManager();
        public MainWindow()
        {
            InitializeComponent();
            userManager.Load();
            if (userManager.Users.Count == 0)
            {
                userManager.AddUser(new User("admin", "admin", "Библиотекарь"));
                userManager.AddUser(new User("guest", "guest", "Гость"));
                userManager.Save();
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            login = loginTextBox.Text;
            password = passwordBox.Password;
            if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите данные");
                return;
            }
            else if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин");
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }            
            var user = userManager.FindUser(login, password);
            if (user != null)
            {
                // Сохраняем текущего пользователя в App свойствах
                Application.Current.Properties["CurrentUser"] = user;

                Menu menu = new Menu();
                menu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}