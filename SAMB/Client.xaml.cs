using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SAMB
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private UserManager userManager = new UserManager();

        public Client()
        {
            InitializeComponent();
            userManager.Load();
            UsersListBox.ItemsSource = userManager.Users;

            // Проверяем права доступа
            CheckUserPermissions();
        }

        private void CheckUserPermissions()
        {
            if (Application.Current.Properties["CurrentUser"] is User currentUser)
            {
                // Если пользователь не библиотекарь, скрываем функционал добавления
                if (currentUser.Role != "Библиотекарь")
                {
                    AddUserButton.Visibility = Visibility.Collapsed;
                    LoginTextBox.IsEnabled = false;
                    PasswordBox.IsEnabled = false;
                    ConfirmPasswordBox.IsEnabled = false;
                    RoleComboBox.IsEnabled = false;

                    // Показываем сообщение
                    ErrorTextBlock.Text = "Только библиотекари могут добавлять пользователей";
                    ErrorTextBlock.Foreground = System.Windows.Media.Brushes.Blue;
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            userManager.Save();
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void OutButton_Click(object sender, RoutedEventArgs e)
        {
            userManager.Save();
            this.Close();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateUserInput(out string errorMessage))
            {
                ErrorTextBlock.Text = errorMessage;
                ErrorTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Проверяем, не существует ли уже пользователь с таким логином
            if (userManager.Users.Any(u => u.Login == login))
            {
                ErrorTextBlock.Text = "Пользователь с таким логином уже существует";
                ErrorTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Создаем нового пользователя
            User newUser = new User(login, password, role);
            userManager.AddUser(newUser);
            userManager.Save();

            // Обновляем список
            UsersListBox.Items.Refresh();

            // Очищаем поля и показываем сообщение об успехе
            ClearForm();
            ErrorTextBlock.Text = "Пользователь успешно добавлен!";
            ErrorTextBlock.Foreground = System.Windows.Media.Brushes.Green;
        }

        private bool ValidateUserInput(out string error)
        {
            error = "";

            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                error += "Логин не может быть пустым\n";
            }
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                error += "Пароль не может быть пустым\n";
            }
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                error += "Пароли не совпадают\n";
            }
            if (PasswordBox.Password.Length < 3)
            {
                error += "Пароль должен быть не менее 3 символов\n";
            }

            return string.IsNullOrEmpty(error);
        }

        private void ClearForm()
        {
            LoginTextBox.Clear();
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            RoleComboBox.SelectedIndex = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            userManager.Save();
            base.OnClosed(e);
        }
    }
}
