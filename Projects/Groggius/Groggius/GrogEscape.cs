using System;
using System.Collections.Generic;
using System.Threading;

namespace Groggius
{
    public static class GrogEscape
    {
        public static void Play(int[] highscores)
        {
            Console.Clear();
            Console.Clear();

            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            int score = 150;

            bool win = false;

            int posX = 1;
            int posY = height - 2;

            string[] adjacentChars = { "", "", "", "" };
            string[,] map = new string[height - 2, width - 2];

            for (int i = 0; i < height - 2; i++)
            {
                for (int j = 0; j < width - 2; j++)
                {
                    map[i, j] = "0";
                }
            }

            CarvePassage(1, height - 2, map);

            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }

                    else if (i == 1 && j == width - 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(" ");
                    }

                    else
                    {
                        if (map[i - 1, j - 1] == "0")
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        else
                        {
                            Console.ResetColor();
                        }

                        Console.Write(map[i - 1, j - 1]);
                    }
                }
            }

            Thread tickThread = new Thread(new ThreadStart(ScoreTick));

            tickThread.Start();

            Console.SetCursorPosition(posX, posY);
            Console.ResetColor();
            Console.Write("G");

            while (score > 0 && !win)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (posY == 1)
                {
                    adjacentChars[0] = "0";
                }

                else
                {
                    adjacentChars[0] = map[posY - 2, posX - 1];
                }

                if (posX == width - 2)
                {
                    adjacentChars[1] = "0";
                }

                else
                {
                    adjacentChars[1] = map[posY - 1, posX];
                }

                if (posY == height - 2)
                {
                    adjacentChars[2] = "0";
                }

                else
                {
                    adjacentChars[2] = map[posY, posX - 1];
                }

                if (posX == 1)
                {
                    adjacentChars[3] = "0";
                }

                else
                {
                    if (posX == 2 && posY == height - 2)
                    {
                        adjacentChars[3] = " ";
                    }

                    else
                    {
                        adjacentChars[3] = map[posY - 1, posX - 2];
                    }
                }

                Console.SetCursorPosition(posX, posY);
                Console.ResetColor();
                Console.Write(" ");

                if (key == ConsoleKey.UpArrow && adjacentChars[0] == " ")
                {
                    posY -= 1;
                }

                else if (key == ConsoleKey.DownArrow && adjacentChars[2] == " ")
                {
                    posY += 1;
                }

                else if (key == ConsoleKey.LeftArrow && adjacentChars[3] == " ")
                {
                    posX -= 1;
                }

                else if (key == ConsoleKey.RightArrow && adjacentChars[1] == " ")
                {
                    posX += 1;
                }

                win = (posX == width - 2 && posY == 1);

                Console.SetCursorPosition(posX, posY);
                Console.ResetColor();
                Console.Write("G");
            }

            if (win)
            {
                if (score > highscores[1])
                {
                    highscores[1] = score;
                }

                Groggius.GameWin(highscores, score, 2);
            }

            else
            {
                if (score > highscores[1])
                {
                    highscores[1] = score;
                }

                Groggius.GameOver(highscores, score, 2);
            }

            tickThread.Abort();

            void ScoreTick()
            {
                while (score > 0 && !win)
                {
                    Console.SetCursorPosition(0, 0);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write($"Score: {score}  ");

                    Thread.Sleep(1000);

                    score -= 1;
                }
            }

            void CarvePassage(int x, int y, string[,] grid)
            {
                const int N = 1;
                const int S = 2;
                const int E = 4;
                const int W = 8;

                Dictionary<int, int> DX = new Dictionary<int, int> { { E, 1 }, { W, -1 }, { N, 0 }, { S, 0 } };
                Dictionary<int, int> DY = new Dictionary<int, int> { { E, 0 }, { W, 0 }, { N, -1 }, { S, 1 } };
                Dictionary<int, int> OPP = new Dictionary<int, int> { { E, W }, { W, E }, { N, S }, { S, N } };

                int[] directions = Shuffle(new int[] { N, S, E, W });

                foreach (int i in directions)
                {
                    int newX = x + DX[i];
                    int newY = y + DY[i];

                    if (newX >= 1 && newX < width - 1 && newY >= 1 && newY < height - 1 && grid[newY - 1, newX - 1] == "0")
                    {
                        if (CheckNeighbors(grid, i, newX, newY))
                        {
                            grid[newY - 1, newX - 1] = " ";

                            CarvePassage(newX, newY, grid);
                        }
                    }
                }
            }

            int[] Shuffle(int[] array)
            {
                Random random = new Random();

                int n = array.Length;

                while (n > 1)
                {
                    n--;

                    int k = random.Next(n + 1);
                    int value = array[k];

                    array[k] = array[n];
                    array[n] = value;
                }

                return array;
            }

            bool CheckNeighbors(string[,] grid, int dir, int x, int y)
            {
                switch (dir)
                {
                    case 1:
                        if (y == 1 && x == 1)
                        {
                            return grid[y - 1, x] == "0";
                        }

                        if (y == 1 && x == width - 2)
                        {
                            return grid[y - 1, x - 2] == "0";
                        }

                        if (y == 1)
                        {
                            return grid[y - 1, x] == "0" && grid[y - 1, x - 2] == "0";
                        }

                        if (x == 1)
                        {
                            return grid[y - 2, x - 1] == "0" && grid[y - 1, x] == "0";
                        }

                        if (x == width - 2)
                        {
                            return grid[y - 2, x - 1] == "0" && grid[y - 1, x - 2] == "0";
                        }

                        return grid[y - 2, x - 1] == "0" && grid[y - 1, x] == "0" && grid[y - 1, x - 2] == "0";

                    case 2:
                        if (y == height - 2 && x == 1)
                        {
                            return grid[y - 1, x] == "0";
                        }

                        if (y == height - 2 && x == width - 2)
                        {
                            return grid[y - 1, x - 2] == "0";
                        }

                        if (y == height - 2)
                        {
                            return grid[y - 1, x] == "0" && grid[y - 1, x - 2] == "0";
                        }

                        if (x == 1)
                        {
                            return grid[y, x - 1] == "0" && grid[y - 1, x] == "0";
                        }

                        if (x == width - 2)
                        {
                            return grid[y, x - 1] == "0" && grid[y - 1, x - 2] == "0";
                        }

                        return grid[y, x - 1] == "0" && grid[y - 1, x] == "0" && grid[y - 1, x - 2] == "0";

                    case 4:
                        if (x == 1 && y == 1)
                        {
                            return grid[y - 1, x] == "0" && grid[y, x - 1] == "0";
                        }

                        if (x == width - 2 && y == height - 2)
                        {
                            return grid[y - 2, x - 1] == "0";
                        }

                        if (x == 1 && y == height - 2)
                        {
                            return grid[y - 1, x] == "0" && grid[y - 2, x - 1] == "0";
                        }

                        if (y == 1 && x == width - 2)
                        {
                            return grid[y, x - 1] == "0";
                        }

                        if (x == width - 2)
                        {
                            return grid[y - 2, x - 1] == "0" && grid[y, x - 1] == "0";
                        }

                        if (y == 1)
                        {
                            return grid[y - 1, x] == "0" && grid[y, x - 1] == "0";
                        }

                        if (y == height - 2)
                        {
                            return grid[y - 1, x] == " " && grid[y - 2, x - 1] == " ";
                        }

                        return grid[y - 1, x] == "0" && grid[y - 2, x - 1] == "0" && grid[y, x - 1] == "0";

                    case 8:
                        if (x == 1 && y == 1)
                        {
                            return grid[y, x - 1] == "0";
                        }

                        if (x == 1 && y == height - 2)
                        {
                            return grid[y - 2, x - 1] == "0";
                        }

                        if (y == 1)
                        {
                            return grid[y - 1, x - 2] == "0" && grid[y, x - 1] == "0";
                        }

                        if (x == 1)
                        {
                            return grid[y - 2, x - 1] == "0" && grid[y, x - 1] == "0";
                        }

                        if (y == height - 2)
                        {
                            return grid[y - 1, x - 2] == "0" && grid[y - 2, x - 1] == "0";
                        }

                        return grid[y - 1, x - 2] == "0" && grid[y - 2, x = 1] == "0" && grid[y, x - 1] == "0";
                }

                return false;
            }
        }
    }
}
