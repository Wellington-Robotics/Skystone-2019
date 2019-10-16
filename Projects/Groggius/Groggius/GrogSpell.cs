using System;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Groggius
{
    public class GrogSpell
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        public static void Play(int[] highscores)
        {
            Console.Clear();

            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Choose difficulty:\n");
            Console.WriteLine("1. Easy\n2. Medium\n3. Hard\n4. Brutal");

            ConsoleKey key = Console.ReadKey(true).Key;

            int maxTime = 60;
            int minLength = 4;
            int maxLength = 4;

            switch (key)
            {
                case ConsoleKey.D1:
                    maxTime = 300;
                    maxLength = 4;

                    break;

                case ConsoleKey.D2:
                    maxTime = 120;
                    maxLength = 6;

                    break;

                case ConsoleKey.D3:
                    maxTime = 60;
                    maxLength = 8;

                    break;

                case ConsoleKey.D4:
                    maxTime = 30;
                    maxLength = 20;

                    break;
            }

            string[] words = File.ReadAllLines("../../Dictionary.txt");

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            int score = 0;
            int time = 0;

            bool gameOver = false;

            Random rand = new Random();

            Thread timeThread = new Thread(new ThreadStart(Time));

            timeThread.Start();

            while (!gameOver)
            {
                Console.Clear();

                string word = "a";
                string shuffledWord = "";

                while (word.Length < minLength || word.Length > maxLength)
                {
                    word = words[rand.Next(words.Length - 1)];
                }

                string current = "";

                do
                {
                    shuffledWord = Groggius.ShuffleWord(word, "");
                } while (shuffledWord == word);

                while (current != word && maxTime - time > 0)
                {
                    Console.Clear();

                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Score: {score} - Time left: {maxTime - time}");

                    Console.SetCursorPosition(width / 2 - word.Length / 2, height / 2);
                    Console.Write(shuffledWord);

                    Console.SetCursorPosition(width / 2 - current.Length / 2, height / 2 + 2);
                    Console.Write(current);

                    ConsoleKey key1 = Console.ReadKey(true).Key;

                    string letter = ((char)key1).ToString().ToLower();

                    if (key1 == ConsoleKey.Backspace && current.Length > 0)
                    {
                        current = current.Remove(current.Length - 1);
                    }

                    else if (Regex.IsMatch(letter, @"[a-z]"))
                    {
                        current += letter;
                    }

                    if (current == word)
                    {
                        score += 1;
                    }
                }
            }

            timeThread.Abort();

            if (score > highscores[4])
            {
                highscores[4] = score;
            }

            Groggius.GameOver(highscores, score, 5);

            void Time()
            {
                while (time <= maxTime)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Score: {score} - Time left: {maxTime - time}");

                    Thread.Sleep(1000);

                    time += 1;
                }

                gameOver = true;

                try
                {
                    CancelIoEx(GetStdHandle(-10), IntPtr.Zero);
                }

                catch
                {
                    Thread.Sleep(0);
                }
            }
        }
    }
}
