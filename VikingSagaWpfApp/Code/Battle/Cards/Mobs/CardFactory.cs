using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public static class CardFactory
    {
        public static BattleCard Create(Type cardType)
        {
            if (!typeof(BattleCard).IsAssignableFrom(cardType))
                throw new ArgumentException("Type must inherit from " + typeof(BattleCard).Name);

            var result = Activator.CreateInstance(cardType);
            return (BattleCard)result;
        }
    }
}
