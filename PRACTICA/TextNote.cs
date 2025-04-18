using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICA
{
    internal class TextNote
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateOnly DateCreate { get; set; }
        public ImportanceNote Importance { get; set; }

        public string GetInfo()
        {
            return $"категория: {Importance} \nдата создания: {DateCreate:d} \nзаголовок: {Title} \nтескт заметки: {Text}";
        }
    }
}
