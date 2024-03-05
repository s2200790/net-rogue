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

        private bool IsValidName(string name)
        {
            return !name.Any(char.IsDigit) && !name.Contains(" ");
        }

        private void CreatePlayer()
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            if (!IsValidName(name))
            {
                Console.WriteLine("Nimessä ei saa olla numeroita tai välilyöntejä");
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
            player.Move(moveX, moveY);
            Console.Clear();
            DrawPlayer();
        }

        public void Run()
        {
            CreatePlayer();

            Console.Clear();
            DrawPlayer();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                HandleInput(keyInfo);
            }
        }
    }
}