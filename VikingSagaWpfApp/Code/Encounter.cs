using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public abstract class Encounter
    {
        public Hero Hero { get; set; }
        public List<Card> PlayableCards { get; set; }

        public Treasure Treasure { get; set; }

        public string PreCombatText { get; set; }
    }
}
