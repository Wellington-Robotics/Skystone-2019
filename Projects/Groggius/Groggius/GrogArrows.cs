using System;
using System.Collections.Generic;
using System.Threading;

namespace Groggius
{
    public static class GrogArrows
    {
        public static void Play(int[] highscores)
        {
            Console.Clear();

            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Choose difficulty:\n");
            Console.WriteLine("1. Easy\n2. Medium\n3. Hard\n4. Brutal");

            ConsoleKey key = Console.ReadKey().Key;

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
            int tick = 0;

            bool gameOver = false;

            List<Groggius.SupportingClasses.Arrow> arrows = new List<Groggius.SupportingClasses.Arrow>();
            List<Groggius.SupportingClasses.Arrow> removeArrows = new List<Groggius.SupportingClasses.Arrow>();

            Random random = new Random();

            Thread keyThread = new Thread(new ThreadStart(KeyPresses));

            keyThread.Start();

            while (!gameOver)
            {
                Console.Clear();

                if (tick == 4)
                {
                    arrows.Add(new Groggius.SupportingClasses.Arrow(random.Next(4), width));

                    tick = 0;
                }

                removeArrows = new List<Groggius.SupportingClasses.Arrow>();

                Console.SetCursorPosition(width / 2 - 10, height - 7);
                Console.Write("--------------------");

                for (int i = height - 6; i < height; i++)
                {
                    Console.SetCursorPosition(width / 2 - 10, i);
                    Console.Write("|                  |");
                }

                foreach (Groggius.SupportingClasses.Arrow arrow in arrows)
                {
                    if (arrow.y >= height - 1)
                    {
                        gameOver = true;
                    }

                    else if (arrow.clicked == true)
                    {
                        removeArrows.Add(arrow);
                    }

                    else
                    {
                        Console.SetCursorPosition(arrow.x, arrow.y);

                        switch (arrow.direction)
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Blue;

                                Console.Write("^");

                                if (arrow.y < height - 1)
                                {
                                    Console.SetCursorPosition(arrow.x, arrow.y + 1);
                                    Console.Write("|");
                                }

                                break;

                            case 1:
                                Console.ForegroundColor = ConsoleColor.Yellow;

                                Console.Write(">");

                                Console.SetCursorPosition(arrow.x - 1, arrow.y);
                                Console.Write("-");

                                break;

                            case 2:
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.Write("v");

                                if (arrow.y > 0)
                                {
                                    Console.SetCursorPosition(arrow.x, arrow.y - 1);
                                    Console.Write("|");
                                }

                                break;

                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;

                                Console.Write("<");

                                Console.SetCursorPosition(arrow.x + 1, arrow.y);
                                Console.Write("-");

                                break;
                        }

                        arrow.y += 1;
                    }
                }

                foreach (Groggius.SupportingClasses.Arrow arrow in removeArrows)
                {
                    score += 1;
                    arrows.Remove(arrow);
                }

                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"Score: {score} - Highscore: {(highscores[3] > score ? highscores[3] : score)}");

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

                    case 3:
                        Thread.Sleep(50);

                        break;
                }

                tick += 1;
            }

            if (score > highscores[2])
            {
                highscores[2] = score;
            }

            Groggius.GameOver(highscores, score, 3);

            keyThread.Abort();

            void KeyPresses()
            {
                while (!gameOver)
                {
                    ConsoleKey key1 = Console.ReadKey(true).Key;

                    Groggius.SupportingClasses.Arrow arrow = arrows[0];

                    if (arrow.y >= height - 7)
                    {
                        int dir = arrows[0].direction;

                        if (key1 == ConsoleKey.LeftArrow && dir == 3)
                        {
                            arrow.clicked = true;
                        }

                        else if (key1 == ConsoleKey.RightArrow && dir == 1)
                        {
                            arrow.clicked = true;
                        }

                        else if (key1 == ConsoleKey.UpArrow && dir == 0)
                        {
                            arrow.clicked = true;
                        }

                        else if (key1 == ConsoleKey.DownArrow && dir == 2)
                        {
                            arrow.clicked = true;
                        }
                    }
                }
            }
        }
    }
}
