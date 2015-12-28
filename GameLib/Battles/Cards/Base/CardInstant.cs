using System;

namespace GameLib.Battles.Cards
{
    public abstract class CardInstant : CardAbility
    {
        public abstract SpellProperty.Result Effect { get; }
    }
}
