using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PRACTICA
{
    static internal class Note
    {
        static string noteSaveFile = "note.json";
        static List<TextNote> textnotes;

        static Note()
        {
            LoadData();
        }
        //загрузка данных
        private static void LoadData()
        {
            if (!File.Exists(noteSaveFile))
                textnotes = new();
            else
                textnotes = JsonSerializer.Deserialize<List<TextNote>>(File.ReadAllText(noteSaveFile));
        }
        //сохранение данных
        public static void SaveData()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonString = JsonSerializer.Serialize(textnotes, options);
            File.WriteAllText(noteSaveFile, jsonString);                                 
        }
        //Добавление новой заметки
        public static void CreateText()
        {
            Console.Write("Введите заголовок заметки: ");
            var title = Console.ReadLine();
            Console.Write("Введите саму заметку: ");
            var text = Console.ReadLine();
            Console.Write("Введите категорию важности заметки " +
                "(1 - низкая важность, 2 - средняя важность, 3 - максимальная важность): ");
            var importance = Convert.ToInt32(Console.ReadLine());
            var dateCreate = DateTime.Now;
            if (importance > 0 && importance < 4 && title != null && text != null)
                textnotes.Add(new TextNote { Title = title, Text = text, Importance = (ImportanceNote)importance, DateCreate = dateCreate.Date });
            else
                Console.WriteLine("некорректные данные продукта");
            SaveData();
        }
        //Изменение категории уже существующей заметки
        public static void ChangeCategory()
        {
            LoadData();
            Console.WriteLine("Введите заголовок заметки, в котором хотите изменить категорию важности: ");
            var title = Console.ReadLine();
            Console.WriteLine("Введите текст заметки, в котором хотите изменить категорию важности: ");
            var text = Console.ReadLine();
            
            //десериализация объекта пользователя (восстановление) - но
            //в памяти будет новый объект пользователя
            //с теми же самыми данными
            foreach (var note in textnotes)
            {                  
                //проверка совпадения введенной пользователем информации и полученной из файла            
                if (title == note.Title && text == note.Text)
                {
                    Console.WriteLine("Введите категорию важности заметки " +
                        "(1 - низкая важность, 2 - средняя важность, 3 - максимальная важность): ");
                    var importance = Convert.ToInt32(Console.ReadLine());
                    if (importance > 0 && importance < 4)
                    {
                        note.Importance = (ImportanceNote)importance;
                        Console.WriteLine(note);
                    }                        
                    else
                        Console.WriteLine("некорректные данные продукта");
                }
            }
            SaveData();
        }  
        public static void ChangeText()
        {
            LoadData();
            Console.WriteLine("Введите заголовок заметки, у которой хотите поменять текст: ");
            var title = Console.ReadLine();
            foreach (var note in textnotes)
            {
                if (title == note.Title)
                {
                    Console.WriteLine("Введите текст: ");
                    var text = Console.ReadLine();
                    if (text != null)
                    {
                        note.Text = text;
                    }
                    else
                    {
                        Console.WriteLine("пустой текст!");
                    }
                }
            }
            SaveData();            
        }
               
        //Вывод заметок по времени создания
        public static void InfoCreateTime()
        {
            textnotes.Sort();
            foreach (var note in textnotes)
            {
                Console.WriteLine(note); 
            }
        }
        //Вывод заметок по категориям
        public static void InfoCategory()
        {
            TextNote note1 = new TextNote();
            note1.SortImportance(ref textnotes);
            foreach (var note in textnotes)
            {
                Console.WriteLine(note);
            }
        }
        //Вывод всех заметок с цветовой идентификацией:
        //самые важные - красным цветом, менее важные - жёлтым, наименее важные - зелёным
        public static void InfoColorCategory()
        {
            foreach (var note in textnotes)
            {
                if (note.Importance == ImportanceNote.CriticalImportant)//проверка на очень важные
                {
                    Console.BackgroundColor = ConsoleColor.Red;                   
                }
                else if (note.Importance == ImportanceNote.TypicalImportant)//проверка на важные
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                }
                else //проверка на неважные
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(note);
            }
        }
    }
}
