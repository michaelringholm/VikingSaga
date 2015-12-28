using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public class Map
    {
        public Map()
        {
            HeroCoordinates = new MapCoordinates();
        }

        public MapCoordinates HeroCoordinates { get; set; }
        public string MapImagePath { get; set; }

        public List<MapLocation> Locations { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        internal static MapLocation GetMapLocation(Map map, double xPos, double yPos)
        {
            var mapLocation = map.Locations.Where(l => (l.Coordinates.X == xPos) && (l.Coordinates.Y == yPos)).SingleOrDefault();
            return mapLocation;
        }

        internal static bool CompareLocations(MapCoordinates mapCoordinates1, MapCoordinates mapCoordinates2)
        {
            return (mapCoordinates1.X == mapCoordinates2.X && (mapCoordinates1.Y == mapCoordinates2.Y));
        }
    }
}
