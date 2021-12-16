using System;
using System.Collections.Generic;

namespace mpSnake
{
    class Program
    {
        struct coordinate
        {
            public int x;
            public int y;
        }

        static coordinate edgeCheck(coordinate s, int height, int width)
        {
            if (s.x < 1)
                s.x = width - 2;
            if (s.x > width - 1)
                s.x = 2;
            if (s.y < 1)
                s.y = height - 2;
            if (s.y > height - 1)
                s.y = 2;

            return s;
        }
        static bool DeathCheck(coordinate s, List<coordinate> snake)
        {
            bool valid = false;
            //checks if x coordinate goes outside window
            for(int i = 1; i < snake.Count; i++)
            {
                coordinate current = snake[i];
                if(s.x == current.x && s.y == current.y)
                {
                    valid = true;
                }
            }
            return valid;
        }
        static void GameOver(List<coordinate> snakeyboi, int height, int width, string name)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.SetCursorPosition(width / 2, height / 2); //position in centre of window
            Console.Write(name + " you are dead! score: " + snakeyboi.Count);
            Console.ReadLine();
        }
        static List<coordinate> GetLong(List<coordinate> snakeBoi, int xMove, int yMove, bool eating)
        {
            coordinate currentCoordinate = snakeBoi[0];
            coordinate oldCoordinate = snakeBoi[0];

            for (int i = 0; i < snakeBoi.Count; i++)
            {
                currentCoordinate = snakeBoi[i];
                if(i == 0)
                {
                    oldCoordinate = snakeBoi[i];
                    currentCoordinate.x += xMove;
                    currentCoordinate.y += yMove;

                    snakeBoi[i] = currentCoordinate;
                }
                else
                {
                    snakeBoi[i] = oldCoordinate;
                    oldCoordinate = currentCoordinate;
                }
            }
            if(eating == true)
                snakeBoi.Add(oldCoordinate);

            return snakeBoi;
        }
        static void Main()
        {
            coordinate snake;

            int height = 50; int width = 120;

            //set initial values
            snake.x = width / 2;
            snake.y = height / 2;
            int xdirection = -1;
            int ydirection = 0;
            Boolean dead = false;
            //fill screen

            Console.SetWindowSize(width, height);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            List<coordinate> snakeBoi = new List<coordinate>();
            snakeBoi.Add(snake); // head
            bool eating = false;
            bool apple = false;
            coordinate appleLoc = new coordinate();
            //Game loop


            // Get username

            string name = GetUsername(height, width);


            while (!dead)
            {
                //has a key been pressed?
                Movement(ref xdirection, ref ydirection);

                //clear white background
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                if(apple == false)
                {
                    appleLoc = SpawnApple(height, width);
                    apple = true;
                }

                eating = CheckApple(snakeBoi[0], appleLoc);
                if (eating == true)
                    apple = false;
                snakeBoi = GetLong(snakeBoi, xdirection, ydirection, eating);

                // Get long

                snakeBoi[0] = edgeCheck(snakeBoi[0], height, width);
                dead = DeathCheck(snakeBoi[0], snakeBoi);


                // draw snake
                Draw(snakeBoi, appleLoc, name, width, height);

            }

            //left while loop - must have died
            GameOver(snakeBoi, height, width, name);
        }

        private static string GetUsername(int height, int width)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.SetCursorPosition((width / 2) - 7, height / 2); //position in centre of window

            Console.WriteLine("Enter username");
            Console.SetCursorPosition((width / 2) - 5, (height / 2) + 1); //position in centre of window

            return Console.ReadLine();
        }

        private static void Movement(ref int xdirection, ref int ydirection)
        {
            //has a key been pressed?
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        // move up
                        xdirection = 0; //no movement in x
                        ydirection = -1; //up movement in y
                        break;
                    case ConsoleKey.DownArrow:
                        // move down
                        xdirection = 0; //no movement in x
                        ydirection = 1; //up movement in y
                        break;
                    case ConsoleKey.LeftArrow:
                        xdirection = -1; //no movement in x
                        ydirection = 0; //up movement in y
                        break;
                    case ConsoleKey.RightArrow:
                        xdirection = 1; //no movement in x
                        ydirection = 0; //up movement in y
                        break;
                }
            }
        }

        private static void Draw(List<coordinate> snake, coordinate appleLoc, string name, int width, int height)
        {
            // DRAW BORDER

            Console.BackgroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{name}: {snake.Count}");

            Console.SetCursorPosition(appleLoc.x, appleLoc.y);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");

            System.Threading.Thread.Sleep(15); // kinda like a awful delta time
            for (int i = 0; i < snake.Count; i++)
            {
                Console.SetCursorPosition(snake[i].x, snake[i].y);
                if (i == 0)
                    Console.BackgroundColor = ConsoleColor.Red;
                else
                    Console.BackgroundColor = ConsoleColor.Blue;

                Console.Write(" ");
            }
            System.Threading.Thread.Sleep(5); //pause
        }

        private static bool CheckApple(coordinate coordinate, coordinate appleLoc)
        {
            bool eat = false;
            coordinate temp = coordinate;
            if (temp.x == appleLoc.x && temp.y == appleLoc.y)
                eat = true;

            return eat;
        }

        private static coordinate SpawnApple(int height, int width)
        {
            
            Random rng = new Random();
            int x = rng.Next(1, width - 1);
            int y = rng.Next(1, height - 1);

            coordinate apple = new coordinate();
            apple.x = x;
            apple.y = y;

            return apple;
        }
    }
}
