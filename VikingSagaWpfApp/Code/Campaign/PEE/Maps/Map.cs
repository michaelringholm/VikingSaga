using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using VikingSaga.Code.Campaign.PEE.Maps;
using System.Diagnostics;
using VikingSaga.Code.Campaign.PEE.Observers;

namespace VikingSaga.Code.Campaign.PEE
{
        // Custom nodes
        // map.FindNode("something").BeforeEnter = someAction(object)
        // BeforeEnter, Enter, BeforeLeave, Leave. Enter also called on Load, BeforeEnter not.

        // Town            : Show town on Enter (also called on load).
        // Map change      : Node may be placed outside of visible area. Change map in BeforeEnter or Enter.
        // Dungeon         : Tiny, start combat on Enter. Leave after combat. Larger: See map change.
        // Dynamic         : Event BeforeLeave : If Persist.Wizard alive. A wizard says 'None shall pass!'. Dialog: Do you want to fight him?
        // Dynamic         : Event Leave. The bridge collapses behind you! Persist.BridgeCollapsed = true (draw 'X') and refuse in BeforeEnter.
        // Dynamic         : Event Enter. If !Quests.HasQuest start NPC dialog OR just show then add quest.


    public abstract class Map
    {
        public static T Create<T>(string serializedMapdata, IGlobalData globalData) where T : Map, new()
        {
            T newMap = new T();
            newMap.SetGlobalData(globalData);
            newMap.SetLocationData(serializedMapdata);
            newMap.Initialize();
            return newMap;
        }

        private void SetGlobalData(IGlobalData globalData)
        {
            GlobalData = globalData;
            MapObserver = globalData.MapObserver;
        }

        public abstract string Id { get; }
        public abstract string Title { get; }
        public readonly List<MapLocationPEE> Locations = new List<MapLocationPEE>();
        public abstract void Enter(MapLocationPEE startLocation);
        public abstract void Initialize();

        protected IMapObserver MapObserver;
        protected IGlobalData GlobalData;

        private void SetLocationData(string strMapData)
        {
            var mapData = LocationSerialization.Deserialize(strMapData);
            foreach(var locationData in mapData.LocationData)
            {
                var mapLocation = new MapLocationPEE(locationData);
                var links = mapData.LocationLinks.Where(l => l.Node1Id == mapLocation.Id);
                mapLocation.Links.AddRange(links);

                Locations.Add(mapLocation);
            }
        }

        public MapLocationPEE GetLocation(string id)
        {
            return Locations.Where(loc => loc.Id == id).FirstOrDefault();
        }

        public IEnumerable<string> GetConnectedLocationIds(MapLocationPEE location)
        {
            var linkedToMe = AllLinks().Where(link => link.Node2Id == location.Id).Select(link => link.Node1Id);
            var linkedFromMe = AllLinks().Where(link => link.Node1Id == location.Id).Select(link => link.Node2Id);
            return linkedToMe.Concat(linkedFromMe);
        }

        public void EnterSpecialLocation(MapLocationPEE owningLocation)
        {
        }

        public void ChangeLocation(MapLocationPEE newLocation)
        {
            MapObserver.OnEnterLocation(newLocation);
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
                throw new ArgumentException("These locations are not linked");

            bool reverse = (link.Node1Id != locationFromId);
            var result = new PathHelper(locationFromId, locationToId, link, reverse);
            return result;
        }

        public IEnumerable<MapLocationLinkData> AllLinks()
        {
            return Locations.SelectMany(loc => loc.Links);
        }
    }
}
