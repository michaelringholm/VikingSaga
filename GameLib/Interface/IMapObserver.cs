using GameLib.Quests;
using GameLib.World;
using GameLib.World.Maps;
using System;

namespace GameLib.Interface
{
    public interface IMapObserver
    {
        void OnEnterLocation(MapLocation location);
        void OnEnterSpecialLocation(WorldData.SpecialLocationEnum specialLocationId);
        void OnLeaveSpecialLocation();
    }
}
