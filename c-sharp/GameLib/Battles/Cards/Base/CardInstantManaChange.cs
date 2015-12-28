using System;

namespace GameLib.Battles.Cards
{
    // Base class for instant/direct heal and direct damage
    public class CardInstantManaChange : CardInstant
    {
        public CardInstantManaChange()
        {
            Name = "CardInstantManaChange";

            CanTargetOwnCard = false;
            CanTargetEnemyCard = false;

            Amount = 0;
        }

        public override SpellProperty.Result Effect { get { return Amount > 0 ? SpellProperty.Result.Positive : SpellProperty.Result.Negative; } }

        public int Amount { get; protected set; }
    }
}
