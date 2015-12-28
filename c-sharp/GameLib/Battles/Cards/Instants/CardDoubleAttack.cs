using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardDoubleAttack : CardInstantSpellProperty
    {
        public CardDoubleAttack()
        {
            Name = "Double attack";

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            Property = SpellProperty.DoubleAttack();
            OnApplyMsg = "<Double Attack>";
        }
    }
}
