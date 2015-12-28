using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardStrength : CardInstantDmgChange
    {
        public CardStrength()
        {
            Name = "Strength";

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            OnApplyMsg = "+1 dmg";

            Amount = 1;
        }
    }
}
