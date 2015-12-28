using GameLib.World.Maps;
using System;

namespace GameLib.DataStore.DTOs
{
    public class WorldLocation
    {
        public string MapId { get; set; }
        public string LocationId { get; set; }
        public bool InsideSpecialLocation { get; set; }
    }
}
