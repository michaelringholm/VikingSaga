using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
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
