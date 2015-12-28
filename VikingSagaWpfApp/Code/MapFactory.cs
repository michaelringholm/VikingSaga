using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public static class MapFactory
    {
        public enum MapEnum { DefaultWorld };

        internal static VikingSaga.Code.Map CreateMap(MapEnum mapEnum)
        {
            if (mapEnum == MapEnum.DefaultWorld)
            {
                var map = CreateDefaultWorld();
                return map;
            }
            else
                throw new Exception("Unknown map type");
        }

        private static Map CreateDefaultWorld()
        {
            var map = new Map();
            map.Width = 840;
            map.Height = 590;
            map.MapImagePath = "maps\\map1_viking_saga.png";
            map.HeroCoordinates.X = 10;
            map.HeroCoordinates.Y = 10;

            // TODO check that the same location isn't added twice
            map.Locations = new List<MapLocation>();
            var random = new Random();
            //var mapLocation1 = new MapLocation { Coordinates = new MapCoordinates{ X = 10, Y = 10 }, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            var mapLocation1 = new MapLocation { Coordinates = new MapCoordinates { X = 10, Y = 10 }, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetEncounter(EncounterFactory.EncounterEnum.LargeRabbit)};
            map.Locations.Add(mapLocation1);
            var mapLocation2 = new MapLocation { Coordinates = new MapCoordinates { X = 12, Y = 12 }, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetEncounter(EncounterFactory.EncounterEnum.LargeRabbit) };
            map.Locations.Add(mapLocation2);
            var mapLocation3 = new MapLocation { Coordinates = new MapCoordinates{ X = 17, Y = 14}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation3);
            var mapLocation4 = new MapLocation { Coordinates = new MapCoordinates{ X = 21, Y = 30}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation4);
            var mapLocation5 = new MapLocation { Coordinates = new MapCoordinates{ X = 31, Y = 40}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation5);
            var mapLocation6 = new MapLocation { Coordinates = new MapCoordinates{ X = 41, Y = 50}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation6);
            var mapLocation7 = new MapLocation { Coordinates = new MapCoordinates{ X = 51, Y = 60}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation7);
            var mapLocation8 = new MapLocation { Coordinates = new MapCoordinates{ X = 56, Y = 70}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation8);
            var mapLocation9 = new MapLocation { Coordinates = new MapCoordinates{ X = 62, Y = 80}, Marker = MapLocation.MarkerTypeEnum.Path, Encounter = EncounterFactory.GetRandomEncounter(random) };
            map.Locations.Add(mapLocation9);
            var mapLocation10 = new MapLocation { Coordinates = new MapCoordinates { X = 65, Y = 86 }, Marker = MapLocation.MarkerTypeEnum.Boss, Encounter = EncounterFactory.GetEncounter(EncounterFactory.EncounterEnum.TrollBoss), IsBossLocation = true, IsBossDefeated = false };
            map.Locations.Add(mapLocation10);

            mapLocation1.ConnectedMapLocations.Add(mapLocation2.Coordinates);
            mapLocation2.ConnectedMapLocations.Add(mapLocation3.Coordinates);
            mapLocation3.ConnectedMapLocations.Add(mapLocation4.Coordinates);
            mapLocation4.ConnectedMapLocations.Add(mapLocation5.Coordinates);
            mapLocation5.ConnectedMapLocations.Add(mapLocation6.Coordinates);
            mapLocation6.ConnectedMapLocations.Add(mapLocation7.Coordinates);
            mapLocation7.ConnectedMapLocations.Add(mapLocation8.Coordinates);
            mapLocation8.ConnectedMapLocations.Add(mapLocation9.Coordinates);
            mapLocation9.ConnectedMapLocations.Add(mapLocation10.Coordinates);
            mapLocation10.ConnectedMapLocations.Add(mapLocation9.Coordinates);
            mapLocation9.ConnectedMapLocations.Add(mapLocation8.Coordinates);
            mapLocation8.ConnectedMapLocations.Add(mapLocation7.Coordinates);
            mapLocation7.ConnectedMapLocations.Add(mapLocation6.Coordinates);
            mapLocation6.ConnectedMapLocations.Add(mapLocation5.Coordinates);
            mapLocation5.ConnectedMapLocations.Add(mapLocation4.Coordinates);
            mapLocation4.ConnectedMapLocations.Add(mapLocation3.Coordinates);
            mapLocation3.ConnectedMapLocations.Add(mapLocation2.Coordinates);
            mapLocation2.ConnectedMapLocations.Add(mapLocation1.Coordinates);
            /*PlotMapLocation(50, 40, "\\markers\\green-highlighted-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a rabbit", CardImageURL = @"mobs\rabbit-150x150.png" } });
            PlotMapLocation(100, 100, "\\markers\\green-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a rabbit", CardImageURL = @"mobs\rabbit-150x150.png" } });
            PlotMapLocation(120, 120, "\\markers\\green-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a wild boar", CardImageURL = @"mobs\wild-boar-900x660.jpg" } });
            PlotMapLocation(140, 120, "\\markers\\green-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a fox", CardImageURL = @"mobs\fox-150x150.png" } });
            PlotMapLocation(140, 140, "\\markers\\green-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a wolf", CardImageURL = @"mobs\wolf-1024x768.jpg" } });*/

            return map;
        }
    }
}
