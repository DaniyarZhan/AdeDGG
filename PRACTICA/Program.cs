﻿namespace PRACTICA
{
    internal class Program
    {
        /*Создать перечисление категории важности заметки
        Создать класс текстовой заметки (категория, дата создания, заголовок, текст заметки, вывод информации по заметке).
        Создать список заметок. Заполнить его программно через специальный метод-генератор
        Создать статичный класс блокнота со статичными методами для:
        1.  Добавления новой заметки
        2.  Изменения категории уже существующей заметки
        3.  Вывода заметок по времени создания
        4.  Вывод заметок по категориям
        5.  Вывод всех заметок с цветовой идентификацией: самые важные - красным цветом, и т.д.
        */
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine
                  ($"0 - выход из приложения\n" +
                   $"1 - добавление новой заметки\n" +
                   $"2 - изменение категории уже существующей заметки\n" +
                   $"3 - вывод заметок по времени создания\n" +
                   $"4 - вывод заметок по категориям\n" +
                   $"5 - вывод всех заметок с цветовой идентификацией: " +
                   $"самые важные - красным цветом, менее важные - жёлтым, наименее важные - зелёным\n");

                Functions functions = (Functions)Convert.ToInt32(Console.ReadLine());

                switch (functions)
                {
                    case Functions.exit: //выход из приложения
                        Note.SaveData(); //сохранение текстовых заметок
                        return;
                    case Functions.createText: //добавление новой заметки
                        Note.CreateText();
                        break;
                    case Functions.changeCategory://изменение категории уже существующей заметки
                        Note.ChangeCategory();
                        break;
                    case Functions.infoCreateTime: //вывод заметок по времени создания
                        Note.InfoCreateTime();
                        break;
                    case Functions.infoCategory: //вывод заметок по категориям
                        Note.InfoCategory();
                        break;
                    case Functions.infoColorCategory: //вывод всех заметок с цветовой идентификацией: самые важные - красным цветом, менее важные - жёлтым, наименее важные - зелёным
                        Note.InfoColorCategory();
                        break;
                }
                Console.ReadKey();
            }
        }
    }
}
