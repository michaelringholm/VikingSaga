using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingSaga.Code.Campaign.PEE.Maps;
using VikingSaga.Code.Campaign.PEE.Observers;
using VikingSaga.Code.Campaign.PEE.WorldNs;

namespace VikingSaga.Code.Campaign.PEE
{
    // World can contain any number of maps.
    // World knows exactly what maps it contains and how they are connected.
    public class World 
    {
        public static World Create(IGlobalData globalData)
        {
            var newWorld = new World(globalData);
            return newWorld;
        }

        public WorldLocationDTO PlayerLocation { get; private set; }

        public GameEventManager GameEventManager { get; private set; }

        private IGlobalData _globalData;
        private IGameDataStore _dataStore;
        private IWorldObserver _worldObserver;

        private MartinsMap _martinsMap;
        // Examples:
        //        private SomeDungeonMap _someDungeonMap; // Could be a dungeon accessed from MartinsMap
        //        private SomeOtherOutsideMap _SomeOtherOutsideMap; // Another outside map connected to MartinsMap.

        private World(IGlobalData globalData)
        {
            _globalData = globalData;
            _dataStore = globalData.DataStore;
            _worldObserver = globalData.WorldObserver;

            CreateMaps();
            Enter();
        }

        public Map GetMap(string mapId)
        {
            if (_martinsMap.Id == mapId)
                return _martinsMap;

            return null;
        }

        private void Enter()
        {
            bool isNewGame = !_dataStore.Exists(DataStoreKey.PlayerWorldLocation);
            if (isNewGame)
            {
                // Start new game
                MapLocationData newGameStartingLocation = _martinsMap.GetLocation("1");
                PlayerLocation = WorldLocationDTO.Create(_martinsMap, newGameStartingLocation);
                WorldLocationDTO.Store(_dataStore, PlayerLocation);
            }

            PlayerLocation = WorldLocationDTO.Get(_dataStore);

            var map = GetMap(PlayerLocation.MapId);
            _worldObserver.OnEnterMap(map);

            var location = map.GetLocation(PlayerLocation.LocationId);
            map.Enter(location);
        }

        private void CreateMaps()
        {
            _martinsMap = Map.Create<MartinsMap>(MartinsMapLocations.Data, _globalData);
        }
    }
}
