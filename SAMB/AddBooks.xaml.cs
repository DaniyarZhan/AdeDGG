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
        public AddBooks()
        {
            InitializeComponent();
            BooksListBox.ItemsSource = library.Books;
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
