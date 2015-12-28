using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardWeaken : CardInstantDmgChange
    {
        public CardWeaken()
        {
            Name = "Weaken";

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            OnApplyMsg = "-1 dmg";

            Amount = -1;
        }
    }
}
