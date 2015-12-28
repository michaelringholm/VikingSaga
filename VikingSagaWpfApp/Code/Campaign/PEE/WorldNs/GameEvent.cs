using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.PEE.WorldNs
{
    public class GameEvent : EventArgs
    {
    }

    public enum SimpleGameEventEnum { SomeBossKilled, EnteredSomeVillage };

    public class SimpleGameEvent : GameEvent
    {
        public SimpleGameEvent(SimpleGameEventEnum @event) { Event = @event; }
        public SimpleGameEventEnum Event { get; private set; }
    }
}
