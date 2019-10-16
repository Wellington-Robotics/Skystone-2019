using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Groggius
{
    public class GroggiusRun
    {
        public static void Main(string[] args)
        {
            int[] highscores = { 0, 0, 0, 0, 0 };

            Console.CursorVisible = false;
            Console.SetWindowSize(80, 24);

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("highscores.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, isoStore);

            if (isoStore.FileExists("highscores.txt"))
            {
                StreamReader reader = new StreamReader(isoStream);

                for (int i = 0; i < highscores.Length; i++)
                {
                    try
                    {
                        highscores[i] = int.Parse(reader.ReadLine());
                    }

                    catch
                    {
                        highscores[i] = 0;
                    }
                }

                reader.Close();
            }

            else
            {
                StreamWriter writer = new StreamWriter(isoStream);

                foreach (int i in highscores)
                {
                    writer.WriteLine(i);
                }

                writer.Close();
            }

            Groggius.Start(highscores);
        }
    }

    public static class Groggius
    {
        public static void Start(int[] highscores)
        {
            bool quit = false;

            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            while (!quit)
            {
                Console.Clear();

                Console.SetCursorPosition(width / 2 - 10, height / 2 - 2);

                Console.Write("Welcome to Groggius");

                Console.SetCursorPosition(width / 2 - 16, height / 2);

                Console.Write("Press 's' to start or 'q' to quit");

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.S)
                {
                    DisplayGameList(highscores);
                }

                else if (key == ConsoleKey.Q)
                {
                    IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                    IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("highscores.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, isoStore);

                    StreamWriter writer = new StreamWriter(isoStream);

                    foreach (int i in highscores)
                    {
                        writer.WriteLine(i);
                    }

                    writer.Close();

                    quit = true;
                }
            }

            Console.Clear();
        }

        public static void GameOver(int[] highscores, int score, int game)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(width / 2 - 5, height / 2 - 3);
            Console.Write("Game Over");

            Console.SetCursorPosition(width / 2 - 4 - score.ToString().Length / 2, height / 2 - 1);
            Console.Write($"Score: {score}");

            Console.SetCursorPosition(width / 2 - 22, height / 2 + 1);
            Console.Write("Press 'p' to play again or 'm' for main menu");

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.P)
                {
                    switch (game)
                    {
                        case 1:
                            GrogDodge.Play(highscores);

                            break;

                        case 2:
                            GrogEscape.Play(highscores);

                            break;

                        case 3:
                            GrogArrows.Play(highscores);

                            break;

                        case 4:
                            GrogBowl.Play(highscores);

                            break;

                        case 5:
                            GrogSpell.Play(highscores);

                            break;
                    }
                }

                else if (key == ConsoleKey.M)
                {
                    Start(highscores);
                }
            } while (key != ConsoleKey.P || key != ConsoleKey.M);
        }

        public static void GameWin(int[] highscores, int score, int game)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;

            Console.Clear();

            Console.SetCursorPosition(width / 2 - 4, height / 2 - 3);

            Console.Write("You Won");

            Console.SetCursorPosition(width / 2 - 4 - score.ToString().Length / 2, height / 2 - 1);

            Console.Write($"Score: {score}");

            Console.SetCursorPosition(width / 2 - 22, height / 2 + 1);

            Console.Write("Press 'p' to play again or 'm' for main menu");

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.P)
                {
                    switch (game)
                    {
                        case 1:
                            GrogDodge.Play(highscores);

                            break;

                        case 2:
                            GrogEscape.Play(highscores);

                            break;

                        case 3:
                            GrogArrows.Play(highscores);

                            break;

                        case 4:
                            GrogBowl.Play(highscores);

                            break;

                        case 5:
                            GrogSpell.Play(highscores);

                            break;
                    }
                }

                else if (key == ConsoleKey.M)
                {
                    Start(highscores);
                }
            } while (key != ConsoleKey.P || key != ConsoleKey.M);
        }

        public static void DisplayGameList(int[] highscores)
        {
            string[] minigames = { "Grog Dodge", "Grog Escape", "Grog Arrows", "Grog Bowl", "Grog Spell" };

            Console.Clear();

            Console.WriteLine("Minigames:\n");

            for (int i = 0; i < minigames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {minigames[i]} - Highscore: {highscores[i]}");
            }

            Console.WriteLine("6. Instructions");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    GrogDodge.Play(highscores);

                    break;

                case ConsoleKey.D2:
                    GrogEscape.Play(highscores);

                    break;

                case ConsoleKey.D3:
                    GrogArrows.Play(highscores);

                    break;

                case ConsoleKey.D4:
                    GrogBowl.Play(highscores);

                    break;

                case ConsoleKey.D5:
                    GrogSpell.Play(highscores);

                    break;

                case ConsoleKey.D6:
                    Instructions();
                    Start(highscores);

                    break;
            }
        }

        static void Instructions()
        {
            Console.Clear();

            Console.WriteLine("Which game would you like instructions for?\n");

            Console.WriteLine("1. Grog Dodge");
            Console.WriteLine("2. Grog Escape");
            Console.WriteLine("3. Grog Arrows");
            Console.WriteLine("4. Grog Bowl");
            Console.WriteLine("5. Grog Spell");

            ConsoleKey key = Console.ReadKey(true).Key;

            Console.Clear();

            switch (key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Use the right and left arrow keys to dodge the objects that fall from the top of the screen. You get a point for every object you succesfully dodge.\n");

                    break;

                case ConsoleKey.D2:
                    Console.WriteLine("Use the up, down, left, and right arrow keys to navigate the maze. The goal is to reach the green box in the top right corner. The more quickly you finish the maze, the more points you get.\n");
                    
                    break;

                case ConsoleKey.D3:
                    Console.WriteLine("Arrows will randomly fall from the top of the screen. When an arrow reaches the box at the bottom of the screen, press the corresponding arrow key to receive a point. The game will end when an arrow reaches the bottom of the screen.\n");

                    break;

                case ConsoleKey.D4:
                    Console.WriteLine("Press the spacebar to roll the ball which is moving back and forth across the bottom of the screen. The ball will roll in a straight line from where it is released. If the ball knocks over any pins, you will receive points. If not, the game ends.\n");

                    break;

                case ConsoleKey.D5:
                    Console.WriteLine("Type on the keyboard to enter the unscrambled version of the scrambled word shown on the screen. Unscramble as many words as possible in the allotted time.\n");

                    break;
            }

            Console.WriteLine("Press any key to return to the game list.");
            Console.ReadLine();
        }

        public static string ShuffleWord(string o, string n)
        {
            if (o.Length == 1)
            {
                return n + o;
            }

            Random random = new Random();

            int rand = random.Next(o.Length - 1);

            return ShuffleWord(o.Remove(rand, 1), n + o[rand]);
        }

        public static class SupportingClasses
        {
            public class Object
            {
                public int x;
                public int y;
                public ConsoleColor color;
                public bool tipped;

                public Object(int x, int y)
                {
                    this.x = x;
                    this.y = y;

                    Random random = new Random();

                    int num = random.Next(5);

                    switch (num)
                    {
                        case 0:
                            color = ConsoleColor.Red;

                            break;

                        case 1:
                            color = ConsoleColor.Yellow;

                            break;

                        case 2:
                            color = ConsoleColor.Green;

                            break;

                        case 3:
                            color = ConsoleColor.Blue;

                            break;

                        case 4:
                            color = ConsoleColor.Magenta;

                            break;

                        case 5:
                            color = ConsoleColor.Black;

                            break;
                    }

                    tipped = false;
                }
            }

            public class Arrow
            {
                public int x;
                public int y;
                public int direction;
                public bool clicked;

                public Arrow(int direction, int width)
                {
                    this.direction = direction;

                    y = 0;
                    clicked = false;

                    switch (direction)
                    {
                        case 0:
                            x = width / 2 - 2;

                            break;

                        case 1:
                            x = width / 2 + 2;

                            break;

                        case 2:
                            x = width / 2 + 4;

                            break;

                        case 3:
                            x = width / 2 - 4;

                            break;
                    }
                }
            }
        }
    }
}