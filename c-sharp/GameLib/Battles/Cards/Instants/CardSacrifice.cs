using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardSacrifice : CardInstantSpellProperty
    {
        public CardSacrifice()
        {
            Name = "Command: Sacrifice!";

            CanTargetOwnCard = true;

            Property = SpellProperty.Sacrifice("<Sacrifice>");
            OnApplyMsg = "Yes, Master!";
        }
    }
}
