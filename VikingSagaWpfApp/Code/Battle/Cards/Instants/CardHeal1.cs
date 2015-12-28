using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardHeal1 : CardInstantHpChange
    {
        public CardHeal1()
        {
            Name = "Heal 1";
            Amount = 1;
            AmountType = HpChangeType.Magic;

            CanTargetEnemyCard = true;
            CanTargetEnemyPlayer = true;
            CanTargetOwnCard = true;
            CanTargetOwnPlayer = true;
        }
    }
}
