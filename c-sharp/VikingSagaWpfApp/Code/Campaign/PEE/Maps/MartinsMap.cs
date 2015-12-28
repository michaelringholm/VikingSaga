using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public class MartinsMap : Map
    {
        public override string Id { get { return this.GetType().Name; } }
        public override string Title { get { return "Martin's Map"; } }

        public override void Initialize()
        {
            // Locations and location links are deserialized from XML. It is only geographical data.
            // Here we hook special behaviour for locations on this map.
            AddGameLogicToLocations();
        }

        private void AddGameLogicToLocations()
        {
            var locationWithCampfire = GetLocation("3");
            locationWithCampfire.Title = "Near a large campfire";
            locationWithCampfire.Description = "Blah blah";

            var campfire = new MartinsMap_Campfire();
            campfire.MapObserver = MapObserver;
            locationWithCampfire.SpecialLocation = campfire;
        }

        public override void Enter(MapLocationPEE startLocation)
        {
            MapObserver.OnEnterLocation(startLocation);
        }
    }
}
