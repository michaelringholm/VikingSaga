using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardRage : CardInstantSpellProperty
    {
        public CardRage()
        {
            Name = "Rage! (1)";

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            Property = SpellProperty.Rage(string.Empty, 2);
            OnApplyMsg = "<Rage!>";
        }
    }
}
