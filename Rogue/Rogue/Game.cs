using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class Game
    {
        private PlayerCharacter player;
        private Map level01;

        private void CreatePlayer()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            if (!IsValidName(name))
            {
                Console.WriteLine("Name cannot contain digits or spaces.");
                CreatePlayer();
                return;
            }

            Console.WriteLine("Choose your species:");
            Console.WriteLine("1. Duck");
            Console.WriteLine("2. Mongoose");
            Console.WriteLine("3. Elf");
            Species species = (Species)(int.Parse(Console.ReadLine()) - 1);

            Console.WriteLine("Choose your role:");
            Console.WriteLine("1. Cook");
            Console.WriteLine("2. Smith");
            Console.WriteLine("3. Rogue");
            Role role = (Role)(int.Parse(Console.ReadLine()) - 1);

            player = new PlayerCharacter(name, species, role);
            Console.Clear();
            Console.WriteLine($"Player created: Name: {player.Name}, Species: {player.Species}, Role: {player.Role}");
            System.Threading.Thread.Sleep(4000);
            Console.Clear();
        }

        private bool IsValidName(string name)
        {
            return !name.Any(char.IsDigit) && !name.Contains(" ");
        }

        private void DrawMap()
        {
            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color

            for (int y = 0; y < level01.MapHeight; y++) // for each row
            {
                for (int x = 0; x < level01.MapWidth; x++) // for each column in the row
                {
                    int index = x + y * level01.MapWidth; // Calculate index of tile at (x, y)
                    int tileId = level01.MapTiles[index]; // Read the tile value at index

                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1:
                            Console.Write("."); // Floor
                            break;
                        case 2:
                            Console.Write("#"); // Wall
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }
        }

        private void DrawPlayer()
        {
            Console.SetCursorPosition(player.X, player.Y);
            Console.Write("@");
        }

        private void HandleInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    MovePlayer(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    MovePlayer(1, 0);
                    break;
            }
        }

        private void MovePlayer(int moveX, int moveY)
        {
            int newX = player.X + moveX;
            int newY = player.Y + moveY;

            if (IsTileWalkable(newX, newY))
            {
                player.Move(moveX, moveY);
                Console.Clear();
                DrawMap();
                DrawPlayer();
            }
        }

        private bool IsTileWalkable(int x, int y)
        {
            int tileId = level01.GetTileAt(x, y);
            return tileId == 1; // Assuming tileId 1 represents a walkable floor
        }

        public void Run()
        {
            CreatePlayer();

            MapLoader loader = new MapLoader();
            level01 = loader.LoadTestMap();

            Console.Clear();
            DrawMap();
            DrawPlayer();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                HandleInput(keyInfo);
            }
        }
    }
}