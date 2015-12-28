using System;
using System.Linq;
using VikingSaga.Code.Campaign.PEE.Observers;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public abstract class MapSpecialLocation
    {
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public abstract void Enter(MapLocationData fromLocation);
        public IMapObserver MapObserver { get; set; }
    }
}
