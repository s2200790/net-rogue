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
        public int[] MapTiles { get; }

        public Map(int mapWidth, int[] mapTiles)
        {
            MapWidth = mapWidth;
            MapTiles = mapTiles;
        }

        public void Draw()
        {
            // Implement Draw method to draw the map
        }
    }
}
