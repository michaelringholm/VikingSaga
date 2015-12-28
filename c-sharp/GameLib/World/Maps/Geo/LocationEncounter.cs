using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib.Encounters;

namespace GameLib.World.Maps.Geo
{
    public class LocationEncounter
    {
        public LocationEncounter(Encounters.Encounter encounter, string locationId)
        {
            this.Encounter = encounter;
            this.LocationId = locationId;
        }
        public string LocationId { get; set; }
        public Encounter Encounter { get; set; }
    }
}
