using System;
using System.Threading;
using System.Data;
using System.Net.Security;

namespace CatchtheTarget
{
    class Program
        {
            static void Main(string[] args)
            {
                // Create player
                var player = new Player();
                Console.WriteLine("What's your name? ");
                player.Name = Console.ReadLine();
                Console.Clear();

                // Show help
                Console.WriteLine("w - up");
                Console.WriteLine("s - down");
                Console.WriteLine("a - left");
                Console.WriteLine("d - right");
                Console.WriteLine("Esc - exit");
                Console.WriteLine("\nPress any key to start");
                Console.ReadKey();

                // X and Y for first map
                int x = 11;
                int y = 11;

                // Set step counter
                int stepCountOver = 0;
                bool exit = false;


                while (true)
                {
                    // Create map and set position for player and target
                    int[,] worldMap = GenerateMap(x, y);
                    int playerPositionX = StartingPositionX(x);
                    int playerPositionY = StartingPositionY(y);

                    // Set target starting location to the random position inside the map
                    Random targetX = new Random();
                    Random targetY = new Random();
                    int targetPositionX = targetX.Next(3, x - 2);
                    int targetPositionY = targetY.Next(4, y - 3);
                    int stepCount = 0;


                    while (true)
                    {
                        // Refreshing map
                        Show(worldMap,
                            x,
                            y,
                            playerPositionX,
                            playerPositionY,
                            player.Name,
                            player.Level,
                            targetPositionX,
                            targetPositionY,
                            stepCountOver);

                        //Read from key map
                        ConsoleKeyInfo input = Console.ReadKey();
                        switch (input.Key)
                        {
                            case ConsoleKey.W:
                                if (playerPositionX > 2)
                                {
                                    playerPositionX -= 1;
                                    stepCount++;
                                    stepCountOver++;
                                }

                                break;

                            case ConsoleKey.S:
                                if (playerPositionX < x - 3)
                                {
                                    playerPositionX += 1;
                                    stepCount++;
                                    stepCountOver++;
                                }

                                break;

                            case ConsoleKey.A:
                                if (playerPositionY > 3)
                                {
                                    playerPositionY -= 1;
                                    stepCount++;
                                    stepCountOver++;
                                }

                                break;

                            case ConsoleKey.D:
                                if (playerPositionY < y - 4)
                                {
                                    playerPositionY += 1;
                                    stepCount++;
                                    stepCountOver++;
                                }

                                break;

                            case ConsoleKey.Escape:
                                exit = true;
                                break;
                            default:
                                break;
                        }

                        // Target move after 2 player moves
                        if (stepCount % 2 == 0)
                        {
                            Random rnd = new Random();

                            switch (rnd.Next(1, 5))
                            {
                                case 1:
                                    if (targetPositionX > 2)
                                    {
                                        targetPositionX -= 1;
                                    }

                                    break;

                                case 2:
                                    if (targetPositionX < x - 3)
                                    {
                                        targetPositionX += 1;
                                    }

                                    break;

                                case 3:
                                    if (targetPositionY > 3)
                                    {
                                        targetPositionY -= 1;
                                    }

                                    break;

                                case 4:
                                    if (targetPositionY < y - 4)
                                    {
                                        targetPositionY += 1;
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }

                        // Collision detection
                        if (playerPositionX == targetPositionX && playerPositionY == targetPositionY)
                        {
                            // Level up
                            player.Level++;

                            // Adding one to map size
                            x++;
                            y++;
                            Console.Clear();
                            Thread.Sleep(500);
                            break;
                        }

                        if (exit)
                            break;
                    }

                    if (exit)
                        break;
                }

                // Show Player info
                Console.WriteLine($"Level: {player.Level}");
                Console.WriteLine($"Steps: {stepCountOver}");
                Console.ReadLine();

            }

            public static int[,] GenerateMap(int x, int y)
            {

                int[,] worldMap = new int[x, y];

                // Creating map - X for height, Y for width
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        // Set border X and Y to 0
                        if (i <= 1 || j <= 2 || i >= x - 2 || j >= y - 3)
                        {
                            worldMap[i, j] = 0;
                        }
                        // Set other X and Y to 1
                        else
                        {
                            worldMap[i, j] = 1;
                        }

                    }
                }

                return worldMap;
            }

            private static void Show(int[,] map,
                int x,
                int y,
                int playerPositionX,
                int playerPositionY,
                string name,
                int level,
                int targetPositionX,
                int targetPositionY,
                int stepCountOver)
            {
                Console.Clear();

                // Showing only nearby positons
                for (int i = playerPositionX - 2; i < playerPositionX + 3; i++)
                {
                    for (int j = playerPositionY - 3; j < playerPositionY + 4; j++)
                    {
                        // Showing Player position as P
                        if (i == playerPositionX && j == playerPositionY)
                        {
                            Console.Write("P");
                        }
                        // Showing Target positon as T
                        else if (i == targetPositionX && j == targetPositionY)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("T");
                        }
                        // Set border value 0 as black and others as white dots
                        else
                        {
                            switch (map[i, j])
                            {
                                case 0:
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(map[i, j]);
                                    break;
                                case 1:
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(".");
                                    break;
                            }


                        }
                        Console.ForegroundColor = ConsoleColor.White;

                        // Locations for Player Info
                        if (i == playerPositionX - 2 && j == playerPositionY + 3)
                        {
                            Console.Write($"    Name: {name}");
                        }
                        else if (i == playerPositionX - 1 && j == playerPositionY + 3)
                        {
                            Console.Write($"    Level: {level}");
                        }
                        else if (i == playerPositionX && j == playerPositionY + 3)
                        {
                            Console.Write($"    Steps: {stepCountOver}");
                        }

                    }
                    Console.WriteLine("");
                }

            }

            private static int StartingPositionX(int x)
            {
                int posx;
                // Set position of Player - center of height
                if (x % 2 == 0)
                {
                    posx = x / 2;
                }
                else
                {
                    posx = (x - 1) / 2;
                }

                return posx;

            }

            private static int StartingPositionY(int y)
            {
                int posy;
                // Set position of Player - center of width
                if (y % 2 == 0)
                {
                    posy = y / 2;
                }
                else
                {
                    posy = (y - 1) / 2;
                }

                return posy;

            }
        }

        public class Player
        {
            public Player()
            {
                Name = "";
                Level = 1;

            }

            public string Name { get; set; }
            public int Level { get; set; }

        }
}

