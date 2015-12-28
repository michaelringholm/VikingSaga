using System;

namespace GameLib.Battles.Cards
{
    // Base class for instant/direct heal and direct damage
    public class CardInstantHpChange : CardInstant
    {
        public CardInstantHpChange()
        {
            Name = "CardInstantHpChange";

            CanTargetOwnCard = true;

            Amount = 0;
            AmountType = HpChangeTypeEnum.Physical;
        }

        public override SpellProperty.Result Effect { get { return Amount > 0 ? SpellProperty.Result.Positive : SpellProperty.Result.Negative; } }

        public int Amount { get; protected set; }
        public HpChangeTypeEnum AmountType { get; protected set; }
    }
}
