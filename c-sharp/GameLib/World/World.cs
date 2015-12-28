using System;
using System.Collections.Generic;
using System.Linq;
using GameLib.Game;
using GameLib.DataStore;
using GameLib.Interface;
using GameLib.World.Maps;
using GameLib.World.Maps.Geo;
using GameLib.DataStore.DTOs;
using Vik.Code.Game.Main.Profiles;
using GameLib.Quests;

namespace GameLib.World
{
    public enum PlayerChangeLocationMethod { LoadGame, Travel, Port };

    public static class WorldFactory
    {
        public static IWorld Create(IGlobalData globalData)
        {
            var newWorld = new World(globalData);
            return newWorld;
        }
    }

    public static class WorldData
    {
        public enum SpecialLocationEnum { Midheim, ValkyrieGraveyard };
    }

    internal class World : IWorld
    {
        public Profile PlayerProfile { get; private set; }
        public GameEventManager GameEventManager { get; private set; }

        private IGlobalData _globalData;
        private IGameDataStore _dataStore;
        private IWorldObserver _worldObserver;

        private StartMap _startMap;
        private SouthMap _southMap;
        private WestMap _westMap;
        private Cave1Map _cave1Map;
        private Labyrinth1Map _labyrinth1Map;
        private QuestInstantiator _questInstantiator;

        private List<Map> _mapList = new List<Map>();

        public World(IGlobalData globalData)
        {
            _globalData = globalData;
            _dataStore = globalData.DataStore;
            _worldObserver = globalData.WorldObserver;
            GameEventManager = globalData.GameEventManager;

            CreateMaps();

            PlayerProfile = new Profile(globalData);
        }

        // IWorld Interface ----->

        void IWorld.Run()
        {
            bool isNewGame = !_dataStore.Exists(DataStoreKey.GameCreatedFlag);
            if (isNewGame)
            {
                CreateNewSaveGame();
            }

            LoadGame();

            ChangePlayerLocationPrivate(PlayerProfile.Data.PlayerLocation.MapId, PlayerProfile.Data.PlayerLocation.LocationId, PlayerChangeLocationMethod.LoadGame);
        }

        private void CreateQuestInstantiator()
        {
            if (_questInstantiator != null)
                _questInstantiator.UnhookEvents();

            _questInstantiator = new QuestInstantiator(PlayerProfile.Data.CompletedQuestTypes, GameEventManager, PlayerProfile);
        }

        private void LoadGame()
        {
            PlayerProfile.LoadGame();
            CreateQuestInstantiator();
        }

        private void CreateNewSaveGame()
        {
            // Flag that this is no longer a new game
            _dataStore.Set(DataStoreKey.GameCreatedFlag);

            // Add some cards to the player profile
            var profileDTO = ProfileDTO.LoadOrCreate<ProfileDTO>(_dataStore, DataStoreKey.PlayerProfile);
            profileDTO.PartyCards = Profile.StartingPartyCards().ToArray();
            profileDTO.DeckCards = Profile.StartingDeckCards().ToList();
            profileDTO.RemainingCards = Profile.StartingRemainingCards().ToList();

            // Pick a player start location in a new game
            MapLocationData newGameStartingLocation = _startMap.GetLocation("Midheim");
            if (newGameStartingLocation == null)
                throw new InvalidOperationException("Cannot start new game, missing start location");

            profileDTO.PlayerLocation = new WorldLocation();
            profileDTO.PlayerLocation.MapId = _startMap.Id;
            profileDTO.PlayerLocation.LocationId = newGameStartingLocation.Id;

            profileDTO.Store();

        }

        void IWorld.SaveAll()
        {
            PlayerProfile.Data.Store();

            _dataStore.ForceCommit();
        }

        IEnumerable<Map> IWorld.GetAllMaps()
        {
            return _mapList;
        }

        Map IWorld.GetMap(string mapId)
        {
            return GetMapPrivate(mapId);
        }

        void IWorld.ChangePlayerLocation(string mapId, string locationId, PlayerChangeLocationMethod method)
        {
            ChangePlayerLocationPrivate(mapId, locationId, method);
        }

        bool IWorld.IsCombatTriggered(string mapId, string sourceLocationId, string targetLocationId, out double t)
        {
            if (new Random().NextDouble() > 0.5)
            {
                t = 0.5;
                return true;
            }
            t = 0;
            return false;
        }

        void IWorld.ExecuteMapLocationAction(MapLocationAction action)
        {
            if (action is ChangePlayerLocationAction)
            {
                var mapChangeAction = (ChangePlayerLocationAction)action;
                ChangePlayerLocationPrivate(mapChangeAction.MapId, mapChangeAction.MapLocationId, PlayerChangeLocationMethod.Travel);
            }
            else if (action is EnterSpecialLocationAction)
            {
                var specialLocationAction = (EnterSpecialLocationAction)action;
                var map = GetMapPrivate(PlayerProfile.Data.PlayerLocation.MapId);
                if (specialLocationAction.Enter)
                {
                    map.EnterSpecialLocation(specialLocationAction.SpecialLocationId);
                }
                else
                {
                    map.LeaveSpecialLocation();
                }
            }
            else
                throw new ArgumentException("Unknown action type: " + typeof(Action).Name);
        }

        void IWorld.EnterSpecialLocation(WorldData.SpecialLocationEnum specialLocationId)
        {
            var map = GetMapPrivate(PlayerProfile.Data.PlayerLocation.MapId);
            map.EnterSpecialLocation(specialLocationId);
        }

        void IWorld.LeaveSpecialLocation()
        {
            var map = GetMapPrivate(PlayerProfile.Data.PlayerLocation.MapId);
            map.LeaveSpecialLocation();
        }

        // <----- IWorld Interface

        Map GetMapPrivate(string mapId)
        {
            return _mapList.Where(map => map.Id == mapId).FirstOrDefault();
        }

        private void ChangePlayerLocationPrivate(string mapId, string locationId, PlayerChangeLocationMethod method)
        {
            var currentMap = GetMapPrivate(PlayerProfile.Data.PlayerLocation.MapId);

            bool isMapChange = mapId != PlayerProfile.Data.PlayerLocation.MapId;
            if (isMapChange || method == PlayerChangeLocationMethod.LoadGame)
            {
                // Location change to new map or load of game
                if (currentMap != null && !currentMap.BeforeLeave())
                    return;

                var newMap = GetMapPrivate(mapId);
                newMap.BeforeEnter();

                PlayerProfile.Data.PlayerLocation.MapId = mapId;
                _worldObserver.OnEnterMap(newMap);

                PlayerProfile.Data.PlayerLocation.LocationId = locationId;
                newMap.ChangeLocation(locationId);

                newMap.AfterEnter();

                if (currentMap != null)
                {
                    currentMap.AfterLeave();
                }
            }
            else
            {
                // Location change on current map
                PlayerProfile.Data.PlayerLocation.LocationId = locationId;
                currentMap.ChangeLocation(locationId);
            }

            GameEventManager.OnDirtyDataEvent(this);
        }

        private void CreateMaps()
        {
            _startMap = Map.Create<StartMap>(StartMap_Locations.Data, _globalData, PlayerProfile);
            _westMap = Map.Create<WestMap>(WestMap_Locations.Data, _globalData, PlayerProfile);
            _southMap = Map.Create<SouthMap>(SouthMap_Locations.Data, _globalData, PlayerProfile);
            _cave1Map = Map.Create<Cave1Map>(Cave1Map_Locations.Data, _globalData, PlayerProfile);
            _labyrinth1Map = Map.Create<Labyrinth1Map>(Labyrinth1Map_Locations.Data, _globalData, PlayerProfile);

            _mapList.Add(_startMap);
            _mapList.Add(_westMap);
            _mapList.Add(_southMap);
            _mapList.Add(_cave1Map);
            _mapList.Add(_labyrinth1Map);
        }
    }
}
