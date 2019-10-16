using System;
using System.Collections.Generic;
using System.Threading;

namespace Groggius
{
    public static class GrogBowl
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

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            int score = 0;

            int posX = width / 2;
            int posY = height - 1;

            int direction = 1;
            int touchingPin = 0;

            bool gameOver = false;
            bool released = false;

            List<Groggius.SupportingClasses.Object> pins = new List<Groggius.SupportingClasses.Object>();

            int k = 0;

            for (int i = 3; i >= 0; i--)
            {
                for (int j = -(2 * i); j <= 2 * i; j += 4)
                {
                    pins.Add(new Groggius.SupportingClasses.Object(width / 2 + 3 * j, k));
                }

                k += 3;
            }

            Thread keyThread = new Thread(new ThreadStart(KeyPresses));

            keyThread.Start();

            while (!gameOver)
            {
                posX = width / 2;
                posY = height - 1;

                while (!released)
                {
                    Console.Clear();

                    Console.SetCursorPosition(0, 0);
                    Console.Write("Score: " + score);

                    foreach (Groggius.SupportingClasses.Object pin in pins)
                    {
                        Console.SetCursorPosition(pin.x, pin.y);
                        Console.Write("0");

                        Console.SetCursorPosition(pin.x, pin.y + 1);
                        Console.Write("|");
                    }

                    Console.SetCursorPosition(posX, posY);
                    Console.Write("G");

                    if (direction == 1)
                    {
                        posX += 1;
                    }

                    else
                    {
                        posX -= 1;
                    }

                    if (posX == width / 2 + 24)
                    {
                        direction = 0;
                    }

                    if (posX == width / 2 - 24)
                    {
                        direction = 1;
                    }

                    switch (difficulty)
                    {
                        case 0:
                            Thread.Sleep(500);

                            break;

                        case 1:
                            Thread.Sleep(250);

                            break;

                        case 2:
                            Thread.Sleep(100);

                            break;

                        case 3:
                            Thread.Sleep(50);

                            break;
                    }
                }

                while (touchingPin == 0 && posY >= 0)
                {
                    Console.Clear();

                    Console.SetCursorPosition(0, 0);
                    Console.Write("Score: " + score);

                    foreach (Groggius.SupportingClasses.Object pin in pins)
                    {
                        Console.SetCursorPosition(pin.x, pin.y);
                        Console.Write("0");

                        Console.SetCursorPosition(pin.x, pin.y + 1);
                        Console.Write("|");

                        if (posX == pin.x && posY == pin.y + 1)
                        {
                            touchingPin = width / 2 - pin.x;
                        }
                    }

                    Console.SetCursorPosition(posX, posY);
                    Console.Write("G");

                    posY -= 1;

                    Thread.Sleep(100);
                }

                if (posY <= 0)
                {
                    Console.Clear();

                    for (int i = 0; i < 4; i++)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Write("Score: " + score);

                        Console.SetCursorPosition(width / 2 - 5, height / 2);
                        Console.Write("You Missed");

                        Thread.Sleep(500);

                        Console.Clear();

                        Console.SetCursorPosition(0, 0);
                        Console.Write("Score: " + score);

                        Thread.Sleep(500);
                    }

                    gameOver = true;
                }

                else
                {
                    string message = "";

                    if (posX == width / 2)
                    {
                        foreach (Groggius.SupportingClasses.Object pin in pins)
                        {
                            pin.tipped = true;
                        }

                        message = "Strike!";
                        score += 10;
                    }

                    switch (touchingPin)
                    {
                        case 6:
                            pins[0].tipped = true;
                            pins[1].tipped = true;
                            pins[2].tipped = true;
                            pins[4].tipped = true;
                            pins[5].tipped = true;
                            pins[7].tipped = true;

                            message = "6 Pins!";
                            score += 6;

                            break;

                        case -6:
                            pins[1].tipped = true;
                            pins[2].tipped = true;
                            pins[3].tipped = true;
                            pins[5].tipped = true;
                            pins[6].tipped = true;
                            pins[8].tipped = true;

                            message = "6 Pins!";
                            score += 6;

                            break;

                        case 12:
                            pins[0].tipped = true;
                            pins[1].tipped = true;
                            pins[4].tipped = true;

                            message = "3 Pins!";
                            score += 3;

                            break;

                        case -12:
                            pins[2].tipped = true;
                            pins[3].tipped = true;
                            pins[6].tipped = true;

                            message = "3 Pins!";
                            score += 3;

                            break;

                        case 18:
                            pins[0].tipped = true;

                            message = "1 Pin!";
                            score += 1;

                            break;

                        case -18:
                            pins[3].tipped = true;

                            message = "1 Pin!";
                            score += 1;

                            break;
                    }

                    Console.Clear();

                    Console.SetCursorPosition(0, 0);
                    Console.Write("Score: " + score);

                    Random random = new Random();

                    int randNum = random.Next(1);

                    foreach (Groggius.SupportingClasses.Object pin in pins)
                    {
                        if (pin.tipped)
                        {
                            if (pin.x < width / 2)
                            {
                                Console.SetCursorPosition(pin.x - 1, pin.y);
                                Console.Write("0");

                                Console.SetCursorPosition(pin.x, pin.y + 1);
                                Console.Write("\\");
                            }

                            else if (pin.x > width / 2)
                            {
                                Console.SetCursorPosition(pin.x + 1, pin.y);
                                Console.Write("0");

                                Console.SetCursorPosition(pin.x, pin.y + 1);
                                Console.Write("/");
                            }

                            else
                            {
                                if (randNum == 0)
                                {
                                    Console.SetCursorPosition(pin.x - 1, pin.y);
                                    Console.Write("0");

                                    Console.SetCursorPosition(pin.x, pin.y + 1);
                                    Console.Write("\\");
                                }

                                else
                                {
                                    Console.SetCursorPosition(pin.x + 1, pin.y);
                                    Console.Write("0");

                                    Console.SetCursorPosition(pin.x, pin.y + 1);
                                    Console.Write("/");
                                }
                            }
                        }

                        else
                        {
                            Console.SetCursorPosition(pin.x, pin.y);
                            Console.Write("0");

                            Console.SetCursorPosition(pin.x, pin.y + 1);
                            Console.Write("|");
                        }
                    }

                    Thread.Sleep(500);

                    Console.Clear();

                    Console.SetCursorPosition(0, 0);
                    Console.Write("Score: " + score);

                    foreach (Groggius.SupportingClasses.Object pin in pins)
                    {
                        if (pin.tipped)
                        {
                            if (pin.x <= width / 2)
                            {
                                Console.SetCursorPosition(pin.x - 2, pin.y + 1);
                                Console.Write("0");

                                Console.SetCursorPosition(pin.x - 1, pin.y + 1);
                                Console.Write("--");
                            }

                            else if (pin.x > width / 2)
                            {
                                Console.SetCursorPosition(pin.x + 2, pin.y + 1);
                                Console.Write("0");

                                Console.SetCursorPosition(pin.x, pin.y + 1);
                                Console.Write("--");
                            }

                            else
                            {
                                if (randNum == 0)
                                {
                                    Console.SetCursorPosition(pin.x - 2, pin.y + 1);
                                    Console.Write("0");

                                    Console.SetCursorPosition(pin.x - 1, pin.y + 1);
                                    Console.Write("--");
                                }

                                else
                                {
                                    Console.SetCursorPosition(pin.x + 2, pin.y + 1);
                                    Console.Write("0");

                                    Console.SetCursorPosition(pin.x, pin.y + 1);
                                    Console.Write("--");
                                }
                            }
                        }

                        else
                        {
                            Console.SetCursorPosition(pin.x, pin.y);
                            Console.Write("0");

                            Console.SetCursorPosition(pin.x, pin.y + 1);
                            Console.Write("|");
                        }
                    }

                    foreach (Groggius.SupportingClasses.Object pin in pins)
                    {
                        pin.tipped = false;
                    }

                    touchingPin = 0;

                    Thread.Sleep(1000);

                    Console.Clear();

                    for (int i = 0; i < 4; i++)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Write("Score: " + score);

                        Console.SetCursorPosition(width / 2 - message.Length / 2, height / 2);
                        Console.Write(message);

                        Thread.Sleep(500);

                        Console.Clear();

                        Console.SetCursorPosition(0, 0);
                        Console.Write("Score: " + score);

                        Thread.Sleep(500);
                    }
                }

                released = false;
                direction = 1;
            }

            keyThread.Abort();

            if (score > highscores[3])
            {
                highscores[3] = score;
            }

            Groggius.GameOver(highscores, score, 4);

            void KeyPresses()
            {
                while (!gameOver)
                {
                    ConsoleKey key1 = Console.ReadKey(true).Key;

                    if (key1 == ConsoleKey.Spacebar)
                    {
                        released = true;
                    }
                }
            }
        }
    }
}