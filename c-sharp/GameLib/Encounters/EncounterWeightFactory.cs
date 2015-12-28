using GameLib.Game;
using GameLib.World.Maps.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Encounters
{
    public class EncounterWeightFactory
    {
        internal static List<EncounterWeight> Create(EncounterFactory.EncounterEnum encounterEnum, int defaultWeight, GameEventManager gameEventManager)
        {
            var encounterWeights = new List<EncounterWeight>();
            var encounterWeight = new EncounterWeight { Encounter = EncounterFactory.Create(encounterEnum, gameEventManager), Weight = defaultWeight };
            encounterWeights.Add(encounterWeight);

            return encounterWeights;
        }
    }
}
