using System;

namespace GameLib.Battles.Cards
{
    public abstract class CardInstantCustom : CardInstant
    {
        public CardInstantCustom()
        {
            Name = "CardInstantCustom";
        }

        public abstract void Execute();
    }
}
