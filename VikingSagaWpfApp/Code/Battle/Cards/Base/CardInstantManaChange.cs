using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    // Base class for instant/direct heal and direct damage
    public class CardInstantManaChange : CardInstant
    {
        public CardInstantManaChange()
        {
            Name = "CardInstantManaChange";

            CanTargetOwnCard = false;
            CanTargetOwnPlayer = false;
            CanTargetEnemyCard = false;
            CanTargetEnemyPlayer = false;

            Amount = 0;
        }

        public int Amount { get; protected set; }
    }
}
