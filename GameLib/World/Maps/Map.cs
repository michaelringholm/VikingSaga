using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using GameLib.Game;
using GameLib.Utility;
using GameLib.Interface;
using GameLib.World.Maps.Geo;
using GameLib.Encounters;
using Vik.Code.Game.Main.Profiles;
using GameLib.DataStore;

namespace GameLib.World.Maps
{
    public abstract class Map
    {
        public static T Create<T>(string serializedMapdata, IGlobalData globalData, Profile playerProfile) where T : Map, new()
        {
            T newMap = new T();
            newMap.SetGlobalData(globalData);
            newMap.PlayerProfile = playerProfile;
            newMap.SetLocationData(serializedMapdata);
            newMap.Initialize();
            return newMap;
        }

        private void SetGlobalData(IGlobalData globalData)
        {
            _mapObserver = globalData.MapObserver;
            _dataStore = globalData.DataStore;
            _gameEventManager = globalData.GameEventManager;
        }

        protected IMapObserver _mapObserver;
        protected IGameDataStore _dataStore;
        protected GameEventManager _gameEventManager;

        protected Profile PlayerProfile { get; set; }

        public abstract string Id { get; }
        public abstract string Title { get; }
        public readonly List<MapLocation> Locations = new List<MapLocation>();
        private List<TravelEncounter> _travelEncounters;
        private List<LocationEncounter> _locationEncounters;
        private Random _rnd = new Random(DateTime.Now.Millisecond);

        private void Initialize()
        {
            // Locations and location links are deserialized from XML. It is only geographical data.
            // Here we hook special behaviour for locations on this map.
            AddGameLogicToLocations();
            _travelEncounters = new List<TravelEncounter>();
            InitTravelEncounters();
            _locationEncounters = new List<LocationEncounter>();
            InitLocationEncounters();
        }

        public Action BeforeEnter = () => { };
        public Action AfterEnter = () => { };
        public Func<bool> BeforeLeave = () => { return true; };
        public Action AfterLeave = () => { };

        private string _currentLocationId;

        private void SetLocationData(string strMapData)
        {
            var mapData = LocationSerialization.Deserialize(strMapData);
            foreach(var locationData in mapData.LocationData)
            {
                var mapLocation = new MapLocation(locationData);
                var links = mapData.LocationLinks.Where(l => l.Node1Id == mapLocation.Id);
                mapLocation.Links.AddRange(links);

                Locations.Add(mapLocation);
            }
        }

        internal void SetLocationText(string locationId, string title, string description)
        {
            var loc = GetLocation(locationId);
            loc.Title = title;
            loc.Description = description;
        }

        internal void AddChangePlayerLocationAction(string locationId, string displayName, string newMapId, string newMapLocationId)
        {
            var locationAction = new ChangePlayerLocationAction { DisplayName = displayName, MapId = newMapId, MapLocationId = newMapLocationId };
            var loc = GetLocation(locationId);
            loc.Actions.Add(locationAction);
        }

        internal void AddEnterSpecialAction(string locationId, string displayName, WorldData.SpecialLocationEnum specialLocationId)
        {
            var locationAction = new EnterSpecialLocationAction { DisplayName = displayName, Enter = true, SpecialLocationId = specialLocationId };
            var loc = GetLocation(locationId);
            loc.Actions.Add(locationAction);
        }

        internal void AddAfterEnterLocationAction(string locationId, Action action)
        {
            var location = GetLocation(locationId);
            location.AfterEnter += action;
        }

        internal void EnterSpecialLocation(WorldData.SpecialLocationEnum specialLocationId)
        {
            _mapObserver.OnEnterSpecialLocation(specialLocationId);
        }

        internal void LeaveSpecialLocation()
        {
            _mapObserver.OnLeaveSpecialLocation();
        }

        public MapLocation GetLocation(string id)
        {
            return Locations.Where(loc => loc.Id == id).FirstOrDefault();
        }

        public IEnumerable<string> GetConnectedLocationIds(MapLocation location)
        {
            var linkedToMe = AllLinks().Where(link => link.Node2Id == location.Id).Select(link => link.Node1Id);
            var linkedFromMe = AllLinks().Where(link => link.Node1Id == location.Id).Select(link => link.Node2Id);
            return linkedToMe.Concat(linkedFromMe);
        }

        internal bool ChangeLocation(string newLocationId)
        {
            var currentLocation = GetLocation(_currentLocationId);
            if (currentLocation != null)
            {
                if (!currentLocation.BeforeLeave())
                    return false;
            }

            var newLocation = GetLocation(newLocationId);

            newLocation.BeforeEnter();

            _mapObserver.OnEnterLocation(newLocation);
            _gameEventManager.OnLocationEnteredEvent(this, new LocationEnteredEventArgs { LocationId = newLocation.Id, Map = this });

            newLocation.AfterEnter();

            if (currentLocation != null)
            {
                currentLocation.AfterLeave();
                _currentLocationId = null;
            }

            return true;
        }

        private bool IsLinkBetween(MapLocationLinkData link, string location1Id, string location2Id)
        {
            bool linksFrom1To2 = (link.Node1Id == location1Id && link.Node2Id == location2Id);
            bool linksFrom2To1 = (link.Node2Id == location1Id && link.Node1Id == location2Id);
            return linksFrom1To2 || linksFrom2To1;
        }

        public PathHelper CreatePathHelper(string locationFromId, string locationToId)
        {
            var locationFrom = GetLocation(locationFromId);
            var locationTo = GetLocation(locationToId);
            MapLocationLinkData link = AllLinks().FirstOrDefault(l => IsLinkBetween(l, locationFromId, locationToId));
            if (link == null)
                throw new ArgumentException("Input locations are not linked");

            bool reverse = (link.Node1Id != locationFromId);
            var result = new PathHelper(locationFromId, locationToId, link, reverse);
            return result;
        }

        public IEnumerable<MapLocationLinkData> AllLinks()
        {
            return Locations.SelectMany(loc => loc.Links);
        }

        protected virtual void AddGameLogicToLocations()
        {
        }

        protected virtual void InitTravelEncounters()
        {
        }

        protected virtual void InitLocationEncounters()
        {
        }

        protected void AddTravelEncounter(TravelEncounter travelEncounter)
        {
            var loc1 = GetLocation(travelEncounter.LocationId1);
            var loc1Connections = GetConnectedLocationIds(loc1);
            if (!loc1Connections.Contains(travelEncounter.LocationId2))
                throw new ArgumentException(string.Format("Invalid TravelEncounter. No connection between [{0}] and [{1}].", travelEncounter.LocationId1, travelEncounter.LocationId2));

            _travelEncounters.Add(travelEncounter);
        }

        protected void AddLocationEncounter(LocationEncounter locationEncounter)
        {
            var loc = GetLocation(locationEncounter.LocationId);

            if (loc != null)
                _locationEncounters.Add(locationEncounter);
            else
                throw new Exception("Location [" + locationEncounter.LocationId + "] was not found!");
        }

        public Encounter GetEncounter(string locationId1, string locationId2, out double t)
        {
            t = 0.0;
            var travelEncounter = _travelEncounters.Find(te => (te.LocationId1 == locationId1 && te.LocationId2 == locationId2) || (te.LocationId2 == locationId1 && te.LocationId1 == locationId2));
            if (travelEncounter == null)
                return null;

            var primaryRoll = _rnd.NextDouble();
            if (primaryRoll > travelEncounter.Risk)
                return null;

            t = travelEncounter.T;

            // Rolled the dice and it's time for an encounter
            double[] summedWeights = new double[travelEncounter.EncounterWeights.Count];

            double sum = 0.0;
            for (int i = 0; i < summedWeights.Count(); ++i)
            {
                sum += travelEncounter.EncounterWeights[i].Weight;
                summedWeights[i] = sum;
            }

            double encounterRoll = _rnd.NextDouble() * sum;
            Encounter chosenEncounter = null;
            for (int i = 0; i < summedWeights.Count(); ++i)
            {
                if (encounterRoll <= summedWeights[i])
                {
                    chosenEncounter = travelEncounter.EncounterWeights[i].Encounter;
                    break;
                }
            }

            if (chosenEncounter == null)
                throw new ArgumentException("Bug! An encounter should always be selected here");

            return chosenEncounter;
        }
    }
}
