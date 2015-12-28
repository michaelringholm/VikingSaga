using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.World.Maps.Geo
{
    public class TravelEncounter
    {
        public TravelEncounter(double risk, double t, string locationId1, string locationId2, List<EncounterWeight> encounterWeights)
        {
            this.Risk = risk;
            this.T = t;
            this.LocationId1 = locationId1;
            this.LocationId2 = locationId2;
            this.EncounterWeights = encounterWeights;
        }

        public double Risk { get; set; }
        public double T { get; set; }
        public string LocationId1 { get; set; }
        public string LocationId2 { get; set; }
        public List<EncounterWeight> EncounterWeights { get; set; }
    }
}
