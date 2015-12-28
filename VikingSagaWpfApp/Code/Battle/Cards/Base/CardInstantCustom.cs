using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
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
