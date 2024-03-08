using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rogue
{
    internal class MapLoader
    {
        public Map LoadMapFromFile(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);

                int mapWidth = lines[0].Length;
                int mapHeight = lines.Length;
                int[] mapTiles = new int[mapWidth * mapHeight];

                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        char tileChar = lines[y][x];
                        mapTiles[x + y * mapWidth] = tileChar switch
                        {
                            '.' => 1, 
                            '#' => 2, 
                            _ => 0,   
                        };
                    }
                }

                return new Map(mapWidth, mapHeight, mapTiles);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File '{fileName}' not found. Loading default map.");
                return LoadTestMap();
            }
        }

        public Map LoadTestMap()
        {
            int mapWidth = 8;
            int[] mapTiles = new int[]
            {
            2, 2, 2, 2, 2, 2, 2, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 2, 2,
            2, 2, 2, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 1, 2,
            2, 2, 2, 2, 2, 2, 2, 2
            };

            return new Map(mapWidth, mapTiles.Length / mapWidth, mapTiles);
        }

        public void TestFileReading(string filename)
        {
            using (StreamReader reader = File.OpenText(filename))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }
            }
        }

        public Map ReadMapFromFile(string filename)
        {
            bool exists = File.Exists(filename);
            if (exists == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents;


            using (StreamReader reader = File.OpenText(filename))
            {
                fileContents = reader.ReadToEnd();
            
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);
            loadedMap.MapHeight = loadedMap.mapTiles.Length / loadedMap.mapWidth;
            return loadedMap;
        }
    }
}
