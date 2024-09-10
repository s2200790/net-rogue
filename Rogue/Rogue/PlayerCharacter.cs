  using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static Rogue.Species;
using static Rogue.Role;

namespace Rogue
{
    public enum Species
    {
        Duck,
        Mongoose,
        Elf
    }

    public enum Role
    {
        Cook,
        Smith,
        Rogue
    }

    internal class PlayerCharacter
    {
        public string Name { get; }
        public Species Species { get; }
        public Role Role { get; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public PlayerCharacter(string name, Species species, Role role)
        {
            Name = name;
            Species = species;
            Role = role;
            X = 2; 
            Y = 2; 
        }

        public void Move(int moveX, int moveY)
        {
            X += moveX;
            Y += moveY;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
        }
    }
}