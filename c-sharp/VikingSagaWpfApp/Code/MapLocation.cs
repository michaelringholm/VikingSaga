using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public class MapLocation
    {
        public MapLocation()
        {
            ConnectedMapLocations = new List<MapCoordinates>();
        }

        public enum MarkerTypeEnum { Path, Hero, Boss, Village };

        public MapCoordinates Coordinates { get; set; }

        public Encounter Encounter { get; set; }

        public MarkerTypeEnum Marker { get; set; }

        public List<MapCoordinates> ConnectedMapLocations { get; set; }

        public bool IsBossLocation { get; set; }

        public bool IsBossDefeated { get; set; }

        internal Campaign.NPC.Midheim Village { get; set; }
    }
}
