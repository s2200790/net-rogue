using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
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
            string name;
            while (true)
            {
                Console.WriteLine("Enter your name:");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit) || name.Contains(" "))
                {
                    Console.WriteLine("Invalid name. Name cannot contain digits or spaces.");
                    continue;
                }
                break;
            }
            Species species;
            while (true)
            {
                Console.WriteLine("Choose your species:");
                Console.WriteLine("1. Duck");
                Console.WriteLine("2. Mongoose");
                Console.WriteLine("3. Elf");
                string vastaus = Console.ReadLine();

                int SpeciesNumber = -1; // Init to invalid value
                                        // Try to parse the answer as a number
                string[] raceNames = Enum.GetNames(typeof(Species));
                if (int.TryParse(vastaus, out SpeciesNumber))
                {
                    // Check that the number is within correct range: 1-3
                    if (SpeciesNumber >= 1 && SpeciesNumber <= raceNames.Length)
                    {
                        // Remember to -1 to get the index
                        // Parse the chosen name as Species enum
                        species = Enum.Parse<Species>(raceNames[SpeciesNumber - 1]);
                        break;
                    }
                }

                if (!Enum.TryParse(vastaus, out species))
                {
                    Console.WriteLine("Invalid species.");
                    continue;
                }
            }
            Role role;
            while (true)
            {

                Console.WriteLine("Choose your role:");
                Console.WriteLine("1. Cook");
                Console.WriteLine("2. Smith");
                Console.WriteLine("3. Rogue");
                string vastaus = Console.ReadLine();

                int RoleNumber = -1; // Init to invalid value
                                        // Try to parse the answer as a number
                string[] raceNames = Enum.GetNames(typeof(Role));
                if (int.TryParse(vastaus, out RoleNumber))
                {
                    // Check that the number is within correct range: 1-3
                    if (RoleNumber >= 1 && RoleNumber <= raceNames.Length)
                    {
                        // Remember to -1 to get the index
                        // Parse the chosen name as Role enum
                        role = Enum.Parse<Role>(raceNames[RoleNumber - 1]);
                        break;
                    }
                }
                
                    Console.WriteLine("Invalid role.");
                    continue;
                
            }

            player = new PlayerCharacter(name, species, role);
            Console.Clear();
            Console.WriteLine($"Player created: Name: {player.Name}, Species: {player.Species}, Role: {player.Role}");
            System.Threading.Thread.Sleep(4000);
            Console.Clear();
        }

        private void DrawMap()
        {
            level01.Draw();
        }

        private void DrawPlayer()
        {
            player.Draw();
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
            return level01.GetTileAt(x, y) == 1; 
        }

        public void Run()
        {
            CreatePlayer();

            MapLoader loader = new MapLoader();
            level01 = loader.ReadMapFromFile("mapfile.json");

            loader.TestFileReading("mapfile.json");

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