using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCSharpQuiz
{
    class QuizGame
    {
        private Question[] questions;

        public QuizGame(string path)
        {
            // Чтение вопросов из файла
            string[] lines = File.ReadAllLines(path);
            questions = new Question[lines.Length / 2];

            for (int i = 0; i < lines.Length; i += 2)
            {
                string questionText = lines[i];
                string answerText = lines[i + 1];
                Question question = new Question(questionText, answerText);
                questions[i / 2] = question;
            }
        }

        public int Start()
        {
            int score = 0;

            Console.WriteLine("--- Игра Викторина ---");

            for (int i = 0; i < questions.Length; i++)
            {
                Question question = questions[i];
                Console.WriteLine("Вопрос {0}:", i + 1);
                Console.WriteLine(question.Text);

                Console.Write("Введите ответ: ");
                string userAnswer = Console.ReadLine();

                if (question.CheckAnswer(userAnswer))
                {
                    score++;
                    Console.WriteLine("Правильно!");
                }
                else
                {
                    Console.WriteLine("Неправильно!");
                }

                Console.WriteLine();
            }

            int perc = 100 / questions.Length * score;
            Console.WriteLine($"Игра окончена! Ваш результат: {score}/{questions.Length} {perc}%");
            return perc;
        }

        public static void SortedFileForPercent(string path)//сортировка по прохождению теста
        {
            string[] lines = File.ReadAllLines(path);

            // Сортировка строк по числу, находящемуся в конце строки
            Array.Sort(lines, (x, y) =>
            {
                // Получение чисел, находящихся в конце строк
                int numberX = int.Parse(x.Substring(x.LastIndexOf(' ') + 1));
                int numberY = int.Parse(y.Substring(y.LastIndexOf(' ') + 1));

                return numberY.CompareTo(numberX);
            });

            // Запись отсортированных строк в файл
            File.WriteAllLines(path, lines);
        }
    }
    class Question
    {
        public string Text { get; }
        private string answer;

        public Question(string text, string answer)
        {
            Text = text;
            this.answer = answer;
        }

        public bool CheckAnswer(string userAnswer)//сравнение ответов
        {
            return String.Equals(userAnswer, answer, StringComparison.OrdinalIgnoreCase);
        }
    }
    internal class Account
    {
        string path = "Information.txt";

        static string _password;
        static string _login;
        static DateTime _date;

        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("Какую викторину выберете?");
            Console.WriteLine("1.Животные\n" +
                "2.Космос\n" +
                "3.История\n" +
                "4.Смешанные вопросы\n" +
                "5.Топ 20 лучших\n" +
                "6.Сменить пароль\n" +
                "7.Выйти из аккаунта\n" +
                "8.Выйти из приложения");

            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.NumPad1)
            {
                Console.Clear();
                Animal();
            }

            if (key == ConsoleKey.NumPad2)
            {
                Console.Clear();
                Space();
            }

            if (key == ConsoleKey.NumPad3)
            {
                Console.Clear();
                History();
            }

            if (key == ConsoleKey.NumPad4)
            {
                Console.Clear();
                AllQuest();
            }

            if (key == ConsoleKey.NumPad5)
            {
                Console.Clear();
                Console.WriteLine("Выберите категорию: \n" +
                    "\t1.История\n" +
                    "\t2.Животные\n" +
                    "\t3.Космос\n" +
                    "\t4.Смешанные\n");
                if (key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Top20Score("InfoHistory.txt");
                }

                if (key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Top20Score("InfoAnimal.txt");
                }

                if (key == ConsoleKey.NumPad3)
                {
                    Console.Clear();
                    Top20Score("InfoSpace.txt");
                }

                if (key == ConsoleKey.NumPad4)
                {
                    Console.Clear();
                    Top20Score("InfoAllQuest.txt");
                }
            }

            if (key == ConsoleKey.NumPad6)
            {
                Console.Clear();
                NewPassword(_login);
            }

            if (key == ConsoleKey.NumPad7)
            {
                Console.Clear();
                MainMenu.Menu();
            }

            if (key == ConsoleKey.NumPad8)
            {
                Console.Clear();
                Console.WriteLine("До свидания");
            }
        }

        void NewPassword(string login)//смена пароля
        {
            Console.WriteLine("Введите старый пароль");
            string lastPassword = Console.ReadLine();
            StreamReader sr = new StreamReader(path);
            string s = sr.ReadToEnd();
            sr.Close();
            if (s.Contains(login + " " + lastPassword) == false)
            {
                while (s.Contains(login + " " + lastPassword) == false)
                {
                    Console.WriteLine("Пароль неверный");
                    lastPassword = Console.ReadLine();
                }
            }
            Console.WriteLine("Введите новый пароль(без пробелов)");
            string newPassword = Console.ReadLine();
            Space(newPassword);

            string searchString = login + " " + lastPassword; // искомая подстрока
            string newString = login + " " + newPassword; // новая подстрока

            // Читаем содержимое файла
            string fileContent = File.ReadAllText(path);
            string fileContent1 = File.ReadAllText("InfoSpace.txt");
            string fileContent2 = File.ReadAllText("InfoHistory.txt");
            string fileContent3 = File.ReadAllText("InfoAnimal.txt");

            // Заменяем подстроку
            string newContent = fileContent.Replace(searchString, newString);
            string newContent1 = fileContent1.Replace(searchString, newString);
            string newContent2 = fileContent2.Replace(searchString, newString);
            string newContent3 = fileContent3.Replace(searchString, newString);

            // Записываем измененное содержимое обратно в файл
            File.WriteAllText(path, newContent);
            File.WriteAllText("InfoSpace.txt", newContent);
            File.WriteAllText("InfoHistory.txt", newContent);
            File.WriteAllText("InfoAnimal.txt", newContent);

            Menu();
        }

        void Space(string temp)//проверка на неналичие пробелы
        {
            for (int i = 0; i < temp.Length; i++)
                if (temp[i] == ' ')
                    while (temp[i] == ' ')
                        temp = Console.ReadLine();
        }

        public void SignUp()//регистрация
        {
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
                fs.Close();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadToEnd();

            Console.WriteLine("Введите свою дату рождения(дд.мм.гггг)");
            string date = Console.ReadLine();
            while (!DateTime.TryParseExact(date, "dd.MM.yyyy", null, DateTimeStyles.None, out _date))

                _date = DateTime.Parse(date);

            Console.WriteLine("Введите  логин для регистрации(без пробелов): ");
            //проверить надо на наличие такого же логина 
            _login = Console.ReadLine();
            Space(_login);
            if (s.Contains(_login) == true)
                while (s.Contains(_login) == true)
                    _login = Console.ReadLine();


            Console.WriteLine("Введите пароль для регистрации(без пробелов): ");
            _password = Console.ReadLine();
            Space(_password);


            sr.Close();
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.Write(_login + " " + _password + " " + _date.ToShortDateString() + "\n");
            }
            using (StreamWriter sw = new StreamWriter("InfoAnimal.txt", true))
            {
                sw.Write(_login + " " + _password + " " + _date.ToShortDateString() + "\n");
            }
            using (StreamWriter sw = new StreamWriter("InfoSpace.txt", true))
            {
                sw.Write(_login + " " + _password + " " + _date.ToShortDateString() + "\n");
            }
            using (StreamWriter sw = new StreamWriter("InfoHistory.txt", true))
            {
                sw.Write(_login + " " + _password + " " + _date.ToShortDateString() + "\n");
            }
        }

        public void SignIn()//вхорд в аккаунт
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadToEnd();

            Console.WriteLine("Введите логин: ");
            string login = Console.ReadLine();
            Space(login);

            if (s.Contains(login) == false)
                while (s.Contains(login) == false)
                {
                    Console.WriteLine("Такого логина не существует!");
                    Console.WriteLine("Введите заново или нажмите 3 чтобы выйти: ");
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.NumPad3)
                    {
                        Console.Clear();
                        MainMenu.Menu();
                    }
                    login = Console.ReadLine();
                    Space(login);
                }


            Console.WriteLine("Введите пароль: ");
            string password = Console.ReadLine();
            Space(password);


            if (s.Contains(login + " " + password) == false)
                while (s.Contains(login + " " + password) == false)
                {
                    Console.WriteLine("Пароль неверный!");
                    password = Console.ReadLine();
                    Space(password);
                }
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains(login + " " + password))
                {
                    Console.WriteLine(line);
                }
            }
        }

        public void SignOut()//выход из аккаунта
        {

        }

        public void DeleteAcc(string login)//удаление аккаунта
        {

        }

        void Animal() //викторина животные
        {
            QuizGame game = new QuizGame("Animal.txt");
            int score = game.Start();
            string path = "InfoAnimal.txt";
            //добавляем процент прохождения
            File.WriteAllLines(path, File.ReadAllLines(path).Select(x => x + (x == _login + " " + _password + " " + _date.ToShortDateString() ? $" {score}" : "")));
            QuizGame.SortedFileForPercent(path);

            Menu();
        }
        void Space() //викторина космос
        {
            QuizGame game = new QuizGame("Space.txt");
            int score = game.Start();
            string path = "InfoSpace.txt";
            //добавляем процент прохождения
            File.WriteAllLines(path, File.ReadAllLines(path).Select(x => x + (x == _login + " " + _password + " " + _date.ToShortDateString() ? $" {score}" : "")));
            QuizGame.SortedFileForPercent(path);

            Menu();
        }
        void History() //викторина история
        {
            QuizGame game = new QuizGame("History.txt");
            int score = game.Start();
            string path = "InfoHistory.txt";
            //добавляем процент прохождения
            File.WriteAllLines(path, File.ReadAllLines(path).Select(x => x + (x == _login + " " + _password + " " + _date.ToShortDateString() ? $" {score}" : "")));
            QuizGame.SortedFileForPercent(path);

            Menu();
        }
        void AllQuest() //викторина все вое темы
        {
            Menu();
            //File.WriteAllLines(@"InfoHistory.txt", File.ReadAllLines(@"InfoHistory.txt").Select(x => x + (x == _login + " " + _password + " " + _date.ToShortDateString() ? $" {score}" : "")));
        }

        public void Top20Score(string path)//топ 20 лучших по процентам
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadLine();

            for (int i = 1; i < 21; i++)
            {
                if (s != null) Console.WriteLine($"{i}){s}");
                s = sr.ReadLine();
            }
            Console.ReadKey();
            Menu();
        }
    }
}