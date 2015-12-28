using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardPoison : CardInstantSpellProperty
    {
        public CardPoison()
        {
            Name = "Poison";

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            Property = SpellProperty.Poisoned(2, 5, "<Poison>");
            OnApplyMsg = "<Poisened>";
        }
    }
}
