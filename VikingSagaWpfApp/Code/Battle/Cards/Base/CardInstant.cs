using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    public abstract class CardInstant : BattleCard
    {
        public abstract SpellProperty.Result Effect { get; }
    }
}
