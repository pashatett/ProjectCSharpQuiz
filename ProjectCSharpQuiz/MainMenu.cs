using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCSharpQuiz
{
    internal class MainMenu
    {
        public static void Menu()
        {
            Account acc = new Account();
            Console.WriteLine("Добро пожаловать в викторину!");
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1)Регистрация\n" +
                                "2)Вход в аккаунт\n" +
                                "3)Выход");

            ConsoleKey key = Console.ReadKey().Key;

            //регистрация
            if (key == ConsoleKey.NumPad1)
            {
                Console.Clear();
                acc.SignUp();
                acc.Menu();
            }

            //вход в аккаунт
            if (key == ConsoleKey.NumPad2)
            {
                Console.Clear();
                acc.SignIn();
                acc.Menu();
            }

            //выход из приложения
            if (key == ConsoleKey.NumPad3)
            {
                Console.Clear();
                Console.WriteLine("До свидания!");
            }


        }
        public void MMenu()
        {
            using (FileStream fs = new FileStream("Information.txt", FileMode.Append)) ;
            Menu();

            
        }
    }
}
