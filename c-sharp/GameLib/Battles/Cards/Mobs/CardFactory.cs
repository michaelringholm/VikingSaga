using System;

namespace GameLib.Battles.Cards
{
    public static class CardFactory
    {
        public static CardBattle Create(Type cardType)
        {
            if (!typeof(CardBattle).IsAssignableFrom(cardType))
                throw new ArgumentException("Type must inherit from " + typeof(CardBattle).Name);

            var result = Activator.CreateInstance(cardType);
            return (CardBattle)result;
        }
    }
}
