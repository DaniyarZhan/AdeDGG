using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private Library library = new Library();
        private User currentUser;
        public Search()
        {
            InitializeComponent();
            // Получаем текущего пользователя
            if (Application.Current.Properties["CurrentUser"] is User user)
            {
                currentUser = user;
                UpdateTitle();
            }
            library.Load();
            ShowAllBooks();
        }
        private void UpdateTitle()
        {
            Title = $"Поиск книг - {currentUser.Login} ({currentUser.Role})";
        }

        private void BasicSearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Введите текст для поиска");
                return;
            }

            var results = library.Books.Where(book =>
                book.Name.ToLower().Contains(searchText) ||
                book.Author.ToLower().Contains(searchText) ||
                book.Genre.ToLower().Contains(searchText)
            ).ToList();

            DisplayResults(results);
        }

        private void AdvancedSearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var genre = GenreFilterTextBox.Text.ToLower();
            var yearFrom = YearFromTextBox.Text;
            var yearTo = YearToTextBox.Text;

            var results = library.Books.Where(book =>
                (string.IsNullOrEmpty(searchText) ||
                 book.Name.ToLower().Contains(searchText) ||
                 book.Author.ToLower().Contains(searchText) ||
                 book.Genre.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(genre) || book.Genre.ToLower().Contains(genre)) &&
                (string.IsNullOrEmpty(yearFrom) || ValidateYear(book.Dateisd) >= int.Parse(yearFrom)) &&
                (string.IsNullOrEmpty(yearTo) || ValidateYear(book.Dateisd) <= int.Parse(yearTo))
            ).ToList();

            DisplayResults(results);
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            GenreFilterTextBox.Clear();
            YearFromTextBox.Clear();
            YearToTextBox.Clear();
            ShowAllBooks();
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAllBooks();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void OutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Вспомогательные методы
        private void DisplayResults(List<Book> results)
        {
            SearchResultsListBox.ItemsSource = results;
            UpdateResultsCount(results.Count);

            // Показываем сообщение если нет результатов
            if (results.Count == 0)
            {
                MessageBox.Show("Книги по вашему запросу не найдены", "Результаты поиска",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ShowAllBooks()
        {
            var allBooks = library.Books.ToList();
            SearchResultsListBox.ItemsSource = allBooks;
            UpdateResultsCount(allBooks.Count);

            if (allBooks.Count == 0)
            {
                MessageBox.Show("В библиотеке пока нет книг", "Информация",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
       
        private void UpdateResultsCount(int count)
        {
            ResultsCountTextBlock.Text = $"Найдено книг: {count}";
        }

        private int ValidateYear(string dateString)
        {
            if (string.IsNullOrEmpty(dateString)) return 0;

            // Пытаемся извлечь год из строки даты
            var yearMatch = Regex.Match(dateString, @"\b\d{4}\b");
            if (yearMatch.Success && int.TryParse(yearMatch.Value, out int year))
            {
                return year;
            }
            return 0;
        }

        // Валидация ввода только чисел для года
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
