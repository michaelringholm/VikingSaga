using System;

namespace GameLib.Battles.Cards
{
    class CardSmallManaPotion : CardInstantManaChange
    {
        public CardSmallManaPotion()
        {
            Name = "A small mana potion";
            Amount = 5;
            ImageUrl = "mana-potion.png";
            Power = 0;
        }
    }
}
