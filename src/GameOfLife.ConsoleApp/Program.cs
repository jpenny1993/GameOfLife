using System;
using System.Threading;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxY = 60;
            const int MaxX = 185;

            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth - 50, Console.LargestWindowHeight - 10);

            var random = new Random();
            var world = new World(MaxX, MaxY);

            do
            {
                // Repopulate the world with random occupiers
                if (world.IsDead()) 
                {
                    for (var i = 0; i < 100; i++)
                    {
                        world.SetEntity(random.Next(MaxX), random.Next(MaxY), true);
                    }
                }

                // Print the changes between the previous frame and the lived frame to console
                world.DrawChanges(((int x, int y) tile, bool occupied) => 
                {
                    var occupier = occupied ? "■" : " ";
                    Console.SetCursorPosition(tile.x, tile.y);
                    Console.Write(occupier);
                });

                // Save the current frame
                world.SaveFrame();

                // Calculate the next frame
                world.LiveFrame();

                Thread.Sleep(150);
            } while (true);
        }
    }
}
