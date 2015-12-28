using System;

namespace GameLib.Battles.Cards
{
    class CardWildBoar : CardBasicMob
    {
        public CardWildBoar()
        {
            Name = "A Wild Boar";
            HpBase = 2;
            DmgBase = 1;
            ImageUrl = @"Data/Gfx/Cards/Mobs/wild-boar.jpg";
            CardFlags = CardFlagsEnum.Minion;
        }
    }
}
