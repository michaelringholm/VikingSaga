using System;
using VikingSaga.Code.Campaign.PEE.Maps;

namespace VikingSaga.Code.Campaign.PEE.Observers
{
    public interface IMapObserver
    {
        void OnEnterLocation(MapLocationPEE location);
        void OnEnterSpecialLocation(MapSpecialLocation specialLocation);
    }
}
