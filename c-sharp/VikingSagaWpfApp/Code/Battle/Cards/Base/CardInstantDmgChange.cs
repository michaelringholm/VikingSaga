using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    public class CardInstantDmgChange : CardInstant
    {
        public CardInstantDmgChange()
        {
            Name = "CardInstantDmgChange";
        }

        public override SpellProperty.Result Effect { get { return Amount > 0 ? SpellProperty.Result.Positive : SpellProperty.Result.Negative; } }

        public int Amount { get; protected set; }
        public string OnApplyMsg { get; protected set; }
    }
}
