using GameLib.Interface;
using GameLib.World;
using GameLib.World.Maps;
using System;
using Vik.Code.Controls;
using Vik.Code.Controls.Towns;
using Vik.Code.Game.Main.Interfaces;

namespace Vik.Code.Game.Main.Observers
{
    public class MapObserver : IMapObserver
    {
        private MapLocation _currentLocation;
        private FakeWindow _currentSpecialLocationWindow;

        void IMapObserver.OnEnterLocation(MapLocation location)
        {
            _currentLocation = location;
            VikGame.MapWindow.SetPlayerLocation(location);
        }

        void IMapObserver.OnEnterSpecialLocation(WorldData.SpecialLocationEnum specialLocationId)
        {
            _currentSpecialLocationWindow = GetSpecialLocationWindowFromId(specialLocationId);
            VikGame.ScreenManager.PushWindow(_currentSpecialLocationWindow);

            ((ISpecialLocation)_currentSpecialLocationWindow).Enter();
        }

        void IMapObserver.OnLeaveSpecialLocation()
        {
            ((ISpecialLocation)_currentSpecialLocationWindow).Leave();

            _currentSpecialLocationWindow.Close(FakeWindow.Result.OK);
        }

        FakeWindow GetSpecialLocationWindowFromId(WorldData.SpecialLocationEnum id)
        {
            FakeWindow result;
            switch (id)
            {
                case WorldData.SpecialLocationEnum.Midheim: result = new LargeVillageWindow(id); break;

                default: throw new ArgumentException("Unknown special location: " + id);
            }

            if (!(result is ISpecialLocation))
                throw new NotImplementedException("A special location must implement interface " + typeof(ISpecialLocation).ToString());

            return result;
        }
    }
}
