using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class Map
    {
        public int MapWidth { get; }
        public int MapHeight { get; }
        public int[] MapTiles { get; }

        public Map(int width, int height, int[] tiles)
        {
            MapWidth = width;
            MapHeight = height;
            MapTiles = tiles;
        }

        public int GetTileAt(int x, int y)
        {
            if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
                return 0; // Out of bounds

            return MapTiles[x + y * MapWidth];
        }

        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color

            for (int y = 0; y < MapHeight; y++) // for each row
            {
                for (int x = 0; x < MapWidth; x++) // for each column in the row
                {
                    int index = x + y * MapWidth; // Calculate index of tile at (x, y)
                    int tileId = MapTiles[index]; // Read the tile value at index

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
    }
}
