using System;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    class MartinsMap_Campfire : MapSpecialLocation
    {
        public MartinsMap_Campfire()
        {
            Title = "A Small Campfire";
            Description = "Four large trolls are eating you!! ...with their eyes. For now...";
        }

        public override void Enter(MapLocationData fromLocation)
        {
            MapObserver.OnEnterSpecialLocation(this);
        }
    }
}
