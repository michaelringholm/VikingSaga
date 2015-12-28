using System;
using System.Collections.Generic;

namespace VikingSaga.Code.Campaign.PEE.WorldNs
{
    public class GameEventManager
    {
        public delegate void GameEventHandler(object sender, GameEvent ge);

        public event GameEventHandler GameEvent;

        public void OnGameEvent(object sender, SimpleGameEventEnum gameEvent)
        {
            GameEvent(sender, new SimpleGameEvent(gameEvent));
        }
    }
}
