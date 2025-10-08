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
    /// Логика взаимодействия для RemoveBooks.xaml
    /// </summary>
    public partial class RemoveBooks : Window
    {
        private Library library = new Library();
        private User currentUser;

        public RemoveBooks()
        {
            InitializeComponent();

            // Получаем текущего пользователя
            if (Application.Current.Properties["CurrentUser"] is User user)
            {
                currentUser = user;
                CheckUserPermissions();
            }

            LoadBooks();
        }

        private void CheckUserPermissions()
        {
            Title = $"Удаление книг - {currentUser.Login} ({currentUser.Role})";
            TitleTextBlock.Text = $"Удаление книг - {currentUser.Role}";

            if (currentUser.IsGuest())
            {
                // Скрываем кнопки удаления для гостя
                BooksListBox.Loaded += (s, e) => DisableDeleteButtons();

                // Показываем сообщение
                var guestMessage = new TextBlock
                {
                    Text = "Режим просмотра: удаление книг доступно только библиотекарям",
                    Foreground = System.Windows.Media.Brushes.Blue,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                // Добавляем сообщение в начало Grid
                var mainGrid = (Grid)Content;
                var rowDefinition = new RowDefinition { Height = GridLength.Auto };
                mainGrid.RowDefinitions.Insert(1, rowDefinition);

                Grid.SetRow(guestMessage, 1);
                Grid.SetColumnSpan(guestMessage, 1);
                mainGrid.Children.Add(guestMessage);
            }
        }

        private void DisableDeleteButtons()
        {
            // Отключаем кнопки удаления для гостя
            foreach (var item in BooksListBox.Items)
            {
                if (BooksListBox.ItemContainerGenerator.ContainerFromItem(item) is System.Windows.Controls.ListBoxItem container)
                {
                    var deleteButton = FindVisualChild<System.Windows.Controls.Button>(container);
                    if (deleteButton != null && deleteButton.Name == "DeleteButton")
                    {
                        deleteButton.IsEnabled = false;
                        deleteButton.Content = "Недоступно";
                        deleteButton.Background = System.Windows.Media.Brushes.Gray;
                    }
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;
                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }
            return null;
        }

        private void LoadBooks()
        {
            library.Load();
            BooksListBox.ItemsSource = library.Books;
            UpdateBooksCount();
        }

        private void UpdateBooksCount()
        {
            BooksCountTextBlock.Text = $"Всего книг: {library.Books.Count}";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка прав доступа
            if (currentUser.IsGuest())
            {
                MessageBox.Show("Удаление книг доступно только библиотекарям", "Ограничение прав",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var button = (Button)sender;
            var bookToRemove = button.Tag as Book;

            if (bookToRemove != null)
            {
                // Подтверждение удаления
                var result = MessageBox.Show($"Вы уверены, что хотите удалить книгу:\n\"{bookToRemove.Name}\" - {bookToRemove.Author}?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    library.RemoveBook(bookToRemove);
                    library.Save();

                    // Обновляем список
                    BooksListBox.Items.Refresh();
                    UpdateBooksCount();

                    MessageBox.Show("Книга успешно удалена", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            library.Save();
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void OutButton_Click(object sender, RoutedEventArgs e)
        {
            library.Save();
            this.Close();
        }        
    }
}
