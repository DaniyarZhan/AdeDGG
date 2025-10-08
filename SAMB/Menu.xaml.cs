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
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private User currentUser;

        public Menu()
        {
            InitializeComponent();

            // Получаем текущего пользователя
            if (Application.Current.Properties["CurrentUser"] is User user)
            {
                currentUser = user;
                UpdateUIForUserRole();
            }
        }

        private void UpdateUIForUserRole()
        {
            // Обновляем заголовок с информацией о пользователе
            Title = $"Главное меню - {currentUser.Login} ({currentUser.Role})";

            // Скрываем кнопки в зависимости от роли
            if (currentUser.IsGuest())
            {
                AddButton.Visibility = Visibility.Collapsed;
                AddClientButton.Visibility = Visibility.Collapsed;
                RemoveButton.Visibility = Visibility.Collapsed;

                // Показываем сообщение для гостя
                var guestText = new TextBlock
                {
                    Text = "Режим гостя: доступен только просмотр и поиск книг",
                    Foreground = Brushes.Blue,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                // Добавляем сообщение в StackPanel
                MainStackPanel.Children.Insert(0, guestText);
            }
        }
                      

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBooks addBooks = new AddBooks();
            addBooks.Show();
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveBooks removeBooks = new RemoveBooks();
            removeBooks.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client();
            client.Show();
            this.Close();
        }
    }
}
