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
    /// Логика взаимодействия для AddBooks.xaml
    /// </summary>
    public partial class AddBooks : Window
    {
        Library library = new();
        private User currentUser;

        public AddBooks()
        {
            InitializeComponent();

            // Получаем текущего пользователя
            if (Application.Current.Properties["CurrentUser"] is User user)
            {
                currentUser = user;
                CheckUserPermissions();
            }

            BooksListBox.ItemsSource = library.Books;
        }

        private void CheckUserPermissions()
        {
            Title = $"Добавление книг - {currentUser.Login} ({currentUser.Role})";

            if (currentUser.IsGuest())
            {
                // Скрываем функционал добавления для гостя
                NameTextBox.IsEnabled = false;
                GenreTextBox.IsEnabled = false;
                AuthorTextBox.IsEnabled = false;
                DateIsdDatePicker.IsEnabled = false;
                AddBookButton.IsEnabled = false;
                AddBookButton.Content = "Доступно только библиотекарям";
                AddBookButton.Background = Brushes.Gray;

                // Просто показываем сообщение
                MessageBox.Show("Режим просмотра: добавление книг доступно только библиотекарям",
                              "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null && currentUser.IsGuest())
            {
                MessageBox.Show("Добавление книг доступно только библиотекарям", "Ограничение прав",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Validate(out string errorMessage)) //если данные не прошли валидацию
            {
                MessageBox.Show(errorMessage); //то показываем сообщение об ошибке(ах)
                return; //завершаем метод
            }

            //если валидация пройдена, то собираем объект книги
            Book book = new Book(
                NameTextBox.Text, //название                 
                GenreTextBox.Text,// жанр книги 
                AuthorTextBox.Text,//автор 
                DateIsdDatePicker.Text //дата издания
                );

            library.AddBook(book); //вызывается метод добавления книги в библиотеку

            BooksListBox.Items.Refresh(); //обновление элементов ListBox чтобы отобразить все изменения
                                          
            // Очищаем поля после успешного добавления
            NameTextBox.Clear();
            GenreTextBox.Clear();
            AuthorTextBox.Clear();
            DateIsdDatePicker.SelectedDate = null;
        }
        #region вспомомагательные_методы
        //метод-валидатор вводимых данных
        private bool Validate(out string error) //валидация входных данных
        {
            error = "";
            

            if (string.IsNullOrEmpty(NameTextBox.Text))  //проверка названия на пустые данные
            {
                error += "Название пусто\n";
            }
            if (string.IsNullOrEmpty(GenreTextBox.Text))  //проверка жанра на пустые данные
            {
                error += "жанр пуст\n";
            }
            if (string.IsNullOrEmpty(AuthorTextBox.Text)) //проверка автора на пустые данные
            {
                error += "поле автора пусто\n";
            }
            if (string.IsNullOrEmpty(DateIsdDatePicker.Text)) //проверка даты на пустые данные
            {
                error += "дата пуста\n";
            }

            if (error.Length > 0)
            {
                return false;
            }
            return true;
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            library.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            library.Load();
            BooksListBox.ItemsSource = library.Books;
        }
    }
}
