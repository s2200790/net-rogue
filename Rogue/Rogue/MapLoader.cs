using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class MapLoader
    {
        public Map LoadTestMap()
        {
            int mapWidth = 8;
            int[] mapTiles = new int[] {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2 };

            return new Map(mapWidth, mapTiles);
        }
    }
}
