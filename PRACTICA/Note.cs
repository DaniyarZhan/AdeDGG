using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

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

        private static void LoadData()
        {
            if (!File.Exists(noteSaveFile))
                textnotes = new();
            else
                textnotes = JsonSerializer.Deserialize<List<TextNote>>(File.ReadAllText(noteSaveFile));
        }

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

        public static void CreateText()
        {
            Console.Write("Введите заголовок заметки: ");
            var headline = Console.ReadLine();
            Console.Write("Введите саму заметку: ");
            var text = Console.ReadLine();
            Console.Write("Введите категорию важности заметки " +
                "(1 - низкая важность, 2 - средняя важность, 3 - максимальная важность): ");
            var importance = (ImportanceNote)Convert.ToInt32(Console.ReadLine());
        }
        public static void ChangeCategory()
        {
            Console.WriteLine("Введите заголовок заметки, в котором хотите изменить категорию важности: ");
            var headline = Console.ReadLine();

        }
        public static void InfoCreateTime()
        {
            
        }
        public static void InfoCategory()
        {

        }
        public static void InfoColorCategory()
        {

        }
    }
}
