using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rogue
{
    internal class Map
    {
        public int mapWidth { get; set; }
        public int MapHeight { get; set; }
        public int[] mapTiles { get; set; }

        public MapLayer[] layers;
        public Map(int width, int height, int[] tiles)
        {
            mapWidth = width;
            MapHeight = height;
            mapTiles = tiles;
        }

        public int GetTileAt(int x, int y)
        {
            if (x < 0 || x >= mapWidth || y < 0 || y >= MapHeight)
                return 0; 

            return mapTiles[x + y * mapWidth];
        }
        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; 
        }

        public void Draw()
        {
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int mapHeight = mapTiles.Length / mapWidth;

            Console.ForegroundColor = ConsoleColor.Gray; 

            for (int y = 0; y < MapHeight; y++) 
            {
                for (int x = 0; x < mapWidth; x++) 
                {
                    int index = x + y * mapWidth; 
                    int tileId = mapTiles[index]; 

                    
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1: 
                            Console.Write("."); 
                            break;
                        case 2:
                            Console.Write("#");
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
