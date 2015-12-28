using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardPriest : CardBasicMob
    {
        public CardPriest()
        {
            Name = "A Priest";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Followers/priest.jpg";

            CardFlags = CardFlagsEnum.Priest;
        }
    }
}
