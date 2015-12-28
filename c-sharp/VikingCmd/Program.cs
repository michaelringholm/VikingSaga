using System;
using System.IO;
using System.Linq;
using VikingSaga.Code.Campaign.PEE;
using VikingSaga.Code.Campaign.PEE.Maps;
using VikingSaga.Code.Campaign.PEE.Observers;
using VikingSaga.Code.Campaign.PEE.WorldNs;

namespace VikingCmd
{

// 
// Sound?
// Gfx?
// EQ inspired?

// Top level objects
// -----------------
// Persist
// Geography
// Battle
// Quests
// GlobalEvents
// NPC
// Player (and enemies? Got lvl, deck, etc.)
// Encounter (semi-random + boss)
// Cards (+ deck / totalDeck)

    class Program
    {
        class Observer : IWorldObserver, IMapObserver
        {
            private Map _currentMap;
            private MapLocationPEE _currentLocation;

            void IWorldObserver.OnEnterMap(Map map)
            {
                _currentMap = map;
                Console.WriteLine(string.Format("Entering map. Id: {0}, Title: {1} (a UI would show the map now)\n", map.Id, map.Title));
            }

            private void ShowLocationOptions(MapLocationPEE location)
            {
                var neighbourIds = _currentMap.GetConnectedLocationIds(location);
                Console.WriteLine(string.Format("You can move to the following locations: {0}", string.Join(",", neighbourIds)));

                while (true)
                {
                    Console.Write("Enter id/command: ");
                    string command = Console.ReadLine().ToLower();
                    if (command == "enter" && _currentLocation.SpecialLocation != null)
                    {
                        _currentLocation.SpecialLocation.Enter(_currentLocation);
                        break;
                    }

                    if (command == _currentLocation.Id)
                    {
                        Console.WriteLine("You are already here, dummy!");
                        continue;
                    }

                    var newLocation = _currentMap.GetLocation(command);
                    if (newLocation == null || !neighbourIds.Contains(command))
                    {
                        Console.WriteLine("Argle-bargle-something?? Try again.");
                        continue;
                    }

                    _currentMap.ChangeLocation(newLocation);
                }
            }

            void IMapObserver.OnEnterLocation(MapLocationPEE location)
            {
                Console.WriteLine(string.Format("You arrive at location Id {0}, Title: {1}, Description: {2}\n", location.Id, location.Title, location.Description));
                _currentLocation = location;
                if (location.SpecialLocation != null)
                {
                    Console.WriteLine(string.Format("You see '{0}'. Type 'enter' to enter.", location.SpecialLocation.Title));
                }
                ShowLocationOptions(_currentLocation);
            }

            public void OnEnterSpecialLocation(MapSpecialLocation specialLocation)
            {
                Console.WriteLine(string.Format("You are at: {0}, Description: {1}", specialLocation.Title, specialLocation.Description));
                Console.WriteLine("Type 'leave' to leave.");
                while (true)
                {
                    string command = Console.ReadLine().ToLower();
                    if (command == "leave")
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var savePath = Environment.CurrentDirectory;
            var saveFile = Path.Combine(savePath, "saveGame.txt");

            // Load existing or create new save game
            IGameDataStore dataStore = File.Exists(saveFile) ? XmlFileGameDataStore.Load(saveFile) : XmlFileGameDataStore.CreateNew(saveFile);

            Observer observer = new Observer();
            GlobalData globalData = new GlobalData();
            globalData.WorldObserver = observer;
            globalData.MapObserver = observer;

            var world = World.Create(globalData);
        }
    }
}

