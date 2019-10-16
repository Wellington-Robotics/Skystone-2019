using System;
using System.Collections.Generic;
using System.Threading;

namespace Groggius
{
    public static class GrogDodge
    {
        public static void Play(int[] highscores)
        {
            Console.Clear();

            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Choose difficulty:\n");
            Console.WriteLine("1. Easy\n2. Medium\n3. Hard\n4. Brutal");

            ConsoleKey key = Console.ReadKey(true).Key;

            int difficulty = 0;

            switch (key)
            {
                case ConsoleKey.D1:
                    difficulty = 0;

                    break;

                case ConsoleKey.D2:
                    difficulty = 1;

                    break;

                case ConsoleKey.D3:
                    difficulty = 2;

                    break;

                case ConsoleKey.D4:
                    difficulty = 3;

                    break;
            }

            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            int score = 0;
            int posX = Console.WindowWidth / 2;

            bool gameOver = false;

            List<Groggius.SupportingClasses.Object> objects = new List<Groggius.SupportingClasses.Object>();

            Random random = new Random();

            Thread keyThread = new Thread(new ThreadStart(KeyPresses));

            keyThread.Start();

            while (!gameOver)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(posX, height - 1);
                Console.Write("G");

                objects.Add(new Groggius.SupportingClasses.Object(random.Next(width), 0));

                List<Groggius.SupportingClasses.Object> removeObjects = new List<Groggius.SupportingClasses.Object>();

                foreach (Groggius.SupportingClasses.Object obj in objects)
                {
                    if (obj.y >= height)
                    {
                        removeObjects.Add(obj);
                    }

                    else if (obj.x == posX && obj.y == height - 1)
                    {
                        gameOver = true;
                    }

                    else
                    {
                        Console.ForegroundColor = obj.color;
                        Console.SetCursorPosition(obj.x, obj.y);
                        Console.Write("|");
                        Console.ForegroundColor = ConsoleColor.Black;

                        obj.y += 1;
                    }
                }

                foreach (Groggius.SupportingClasses.Object obj in removeObjects)
                {
                    objects.Remove(obj);
                    score += 1;
                }

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.Write($"Score: {score} - Highscore: {(highscores[0] > score ? highscores[0] : score)}");

                switch (difficulty)
                {
                    case 0:
                        Thread.Sleep(500);

                        break;

                    case 1:
                        Thread.Sleep(300);

                        break;

                    case 2:
                        Thread.Sleep(100);

                        break;

                    default:
                        Thread.Sleep(50);

                        break;
                }
            }

            keyThread.Abort();

            if (score > highscores[0])
            {
                highscores[0] = score;
            }

            Groggius.GameOver(highscores, score, 1);

            void KeyPresses()
            {
                while (!gameOver)
                {
                    ConsoleKey key1 = Console.ReadKey(true).Key;

                    if (key1 == ConsoleKey.LeftArrow && posX > 0)
                    {
                        posX -= 1;

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(posX, height - 1);
                        Console.Write("G");

                        Console.SetCursorPosition(posX + 1, height - 1);
                        Console.Write(" ");
                    }

                    else if (key1 == ConsoleKey.RightArrow && posX < width - 1)
                    {
                        posX += 1;

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(posX, height - 1);
                        Console.Write("G");

                        Console.SetCursorPosition(posX - 1, height - 1);
                        Console.Write(" ");
                    }
                }
            }
        }
    }
}