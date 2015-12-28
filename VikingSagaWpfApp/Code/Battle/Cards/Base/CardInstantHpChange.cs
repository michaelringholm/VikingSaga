using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    // Base class for instant/direct heal and direct damage
    public class CardInstantHpChange : CardInstant
    {
        public CardInstantHpChange()
        {
            Name = "CardInstantHpChange";

            CanTargetOwnCard = true;
            CanTargetOwnPlayer = true;

            CanTargetEnemyCard = true;
            CanTargetEnemyPlayer = true;

            Amount = 0;
            AmountType = HpChangeType.Physical;
        }

        public int Amount { get; protected set; }
        public HpChangeType AmountType { get; protected set; }
    }
}
