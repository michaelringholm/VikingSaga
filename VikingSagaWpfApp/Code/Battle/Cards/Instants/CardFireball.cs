using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public class CardFireball : CardInstantHpChange
    {
        public CardFireball()
        {
            Name = "Fireball";
            Amount = -1;
            AmountType = HpChangeType.Fire;

            CanTargetEnemyCard = true;
            CanTargetEnemyPlayer = true;
            CanTargetOwnCard = true;
            CanTargetOwnPlayer = true;
        }
    }
}
