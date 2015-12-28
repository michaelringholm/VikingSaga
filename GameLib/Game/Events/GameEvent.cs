using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Game
{
    public class GameEvent : EventArgs
    {
    }

    public enum SimpleGameEventEnum { SaveAll, SomeBossKilled, EnteredSomeVillage };

    public class SimpleGameEvent : GameEvent
    {
        public SimpleGameEvent(SimpleGameEventEnum eventId) { Event = eventId; }
        public SimpleGameEventEnum Event { get; private set; }
    }
}
