using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMB
{
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Dateisd { get; set; }
        public Book(string name,  string genre, string author, string date)
        {
            Name = name;
            Author = author;
            Genre = genre;
            Dateisd = date;
        }
        public override string ToString()
        {
            return $"Автор {Author} Название книги: {Name} Жанр: {Genre} Дата издания: {Dateisd}";
        }
    }
}
