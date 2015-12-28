using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingSagaWpfApp.Code.Battle;

namespace VikingSaga.Code
{
    public class CombatDeck
    {
        public List<Card> RemainingCards { get; set; }
        public List<Card> DefeatedCards { get; set; }
    }
}
