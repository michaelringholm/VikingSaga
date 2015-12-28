using System;
using VikingSaga.Code.Campaign.PEE.DataStore;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public class WorldLocationDTO : DTO
    {
        public static void Store(IGameDataStore store, WorldLocationDTO playerLocation)
        {
            store.Set(DataStoreKey.PlayerWorldLocation, playerLocation);
        }

        public static WorldLocationDTO Get(IGameDataStore store)
        {
            return (WorldLocationDTO)store.Get(DataStoreKey.PlayerWorldLocation);
        }

        public static WorldLocationDTO Create(Map map, MapLocationData location)
        {
            return new WorldLocationDTO() { MapId = map.Id, LocationId = location.Id };
        }

        private WorldLocationDTO() { }

        public string MapId { get; set; }
        public string LocationId { get; set; }
        public bool InsideSpecialLocation { get; set; }
    }
}
