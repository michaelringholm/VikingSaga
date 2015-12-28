using System;
using GameLib.DataStore;
using GameLib.Interface;

namespace GameLib.Game
{
    public interface IGlobalData
    {
        GameEventManager GameEventManager { get; }
        IGameDataStore DataStore { get; }
        IWorldObserver WorldObserver { get; }
        IMapObserver MapObserver { get; }
    }

    public class GlobalData : IGlobalData
    {
        public GlobalData()
        {
            GameEventManager = new GameEventManager();
        }

        public GameEventManager GameEventManager { get; private set; }
        public IGameDataStore DataStore { get; set; }
        public IWorldObserver WorldObserver { get; set; }
        public IMapObserver MapObserver { get; set; }
    }
}
