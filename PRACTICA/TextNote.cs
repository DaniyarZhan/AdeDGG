using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICA
{
    internal class TextNote : IComparable
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreate { get; set; }
        public ImportanceNote Importance { get; set; }
        public override string ToString()
        {
            return $"категория: {Importance} \nдата создания: {DateCreate:d} \nзаголовок: {Title} \nтекст заметки: {Text}";
        }
        public int CompareTo(object? other ) //сортировка по времени создания
        {
            if (other is TextNote textNote )
            {
                return DateCreate.CompareTo(textNote.DateCreate);
            }            
            return 0;
        }
        public void SortImportance(ref List<TextNote> notes) //сортировка по важности
        {
            List<TextNote> textNotes = new List<TextNote>();
            ImportanceNote important = ImportanceNote.NotImportant;
            foreach (var note in notes)
            {
                if (note.Importance == important)
                {
                    textNotes.Add(note);
                }
            }
            important = ImportanceNote.TypicalImportant;
            foreach (var note in notes)
            {
                if (note.Importance == important)
                {
                    textNotes.Add(note);
                }
            }
            important = ImportanceNote.CriticalImportant;
            foreach (var note in notes)
            {
                if (note.Importance == important)
                {
                    textNotes.Add(note);
                }
            }
            notes = textNotes;
        }
    }
}
