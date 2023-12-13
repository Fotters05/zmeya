using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class SnakeGame
{
    static int width = 40;
    static int height = 20;

    static int snakeX, snakeY; 
    static List<int> snakeTailX = new List<int>(); 
    static List<int> snakeTailY = new List<int>();
    static int foodX, foodY; 

    static int score = 0;

    static string direction = "RIGHT"; 
    static bool gameover = false;

    static Random rnd = new Random();
    static Thread moveSnakeThread;

    static bool startGame = false;

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(width + 1, height + 1);
        Console.SetBufferSize(width + 1, height + 1);

        Initialize();

        Console.WriteLine("Press any key to start...");


        Console.ReadKey(true);

        startGame = true;

        moveSnakeThread = new Thread(MoveSnake);
        moveSnakeThread.Start();

        do
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && direction != "DOWN")
                    direction = "UP";
                else if (key == ConsoleKey.DownArrow && direction != "UP")
                    direction = "DOWN";
                else if (key == ConsoleKey.LeftArrow && direction != "RIGHT")
                    direction = "LEFT";
                else if (key == ConsoleKey.RightArrow && direction != "LEFT")
                    direction = "RIGHT";
            }
        } while (!gameover && startGame);


        Console.SetCursorPosition(width / 2 - 5, height / 2);
        Console.WriteLine("Game Over! Score: " + score);
        Console.ReadLine();

        moveSnakeThread.Join(); 
    }

    static void Initialize()
    {
        snakeX = width / 2;
        snakeY = height / 2;
        GenerateFood();
    }

    static void GenerateFood()
    {
        foodX = rnd.Next(1, width);
        foodY = rnd.Next(1, height);
    }

    static void MoveSnake()
    {
        while (!gameover)
        {
            switch (direction)
            {
                case "UP":
                    snakeY--;
                    break;
                case "DOWN":
                    snakeY++;
                    break;
                case "LEFT":
                    snakeX--;
                    break;
                case "RIGHT":
                    snakeX++;
                    break;
            }

         
            if (snakeX <= 0 || snakeY <= 0 || snakeX >= width - 1 || snakeY >= height - 1)
            {
                gameover = true;
            }
            else
            {
               
                if (snakeX == foodX && snakeY == foodY)
                {
                    score++;
                    GenerateFood();
                    snakeTailX.Add(snakeX);
                    snakeTailY.Add(snakeY);
                }

               
                snakeTailX.Add(snakeX);
                snakeTailY.Add(snakeY);

                
                while (snakeTailX.Count > score)
                {
                    snakeTailX.RemoveAt(0);
                    snakeTailY.RemoveAt(0);
                }

                
                DrawGame();
                Thread.Sleep(100);
            }
        }
    }

    static void DrawGame()
    {
        Console.Clear();

        
        for (int i = 0; i < width; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("-");

            Console.SetCursorPosition(i, height - 1);
            Console.Write("-");
        }

        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("|");

            Console.SetCursorPosition(width - 1, i);
            Console.Write("|");
        }

       
        Console.SetCursorPosition(foodX, foodY);
        Console.Write("$");

        
        Console.SetCursorPosition(snakeX, snakeY);
        Console.Write("&");

        
        for (int i = 0; i < snakeTailX.Count(); i++)
        {
            Console.SetCursorPosition(snakeTailX[i], snakeTailY[i]);
            Console.Write("&");
        }
    }
}