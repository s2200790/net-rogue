using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
// using TurboMapReader;

namespace Rogue
{
    internal class MapLoader
    {

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
                        break;
                    }
                    Console.WriteLine(line);
                }
            }
        }
        //public Map LoadMapFromTiledFile(string filename)
        //{
            //TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile(filename);
            
            //if (loadedTileMap != null)
            //{
                //Console.WriteLine($"Loaded TileMap {loadedTileMap}");
                //int mapWidth = loadedTileMap.width;
                //int mapHeight = loadedTileMap.height;
                //List<int> mapTiles = new List<int>();

                //foreach (TurboMapReader.MapLayer layer in loadedTileMap.layers)
                //{
                //    foreach (int tile in layer.data)
                  //  {
                //        mapTiles.Add(tile - 1);
                //    }
              //  }
                //Map loadedMap = new Map(mapWidth, mapHeight, mapTiles.ToArray());
                
                //return loadedMap;
            //}
            //return null;
        //}
            public Map ReadMapFromFile(string filename)
            {
                bool exists = File.Exists(filename);
                if (exists == false)
                {
                    Console.WriteLine($"File {filename} not found");
                    return LoadTestMap();
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
