using GameLib.DataStore.DTOs;
using GameLib.Game;
using GameLib.World;
using GameLib.World.Maps;
using System.Collections.Generic;
using Vik.Code.Game.Main.Profiles;

namespace GameLib.Interface
{
    public interface IWorld
    {
        void Run();
        void SaveAll();

        Profile PlayerProfile { get; }
        GameEventManager GameEventManager { get; }

        IEnumerable<Map> GetAllMaps();
        Map GetMap(string mapId);

        void ExecuteMapLocationAction(MapLocationAction action);

        void ChangePlayerLocation(string mapId, string locationId, PlayerChangeLocationMethod method);
        bool IsCombatTriggered(string mapId, string sourceLocationId, string targetLocationId, out double t);

        void EnterSpecialLocation(WorldData.SpecialLocationEnum specialLocationId);
        void LeaveSpecialLocation();
    }
}
