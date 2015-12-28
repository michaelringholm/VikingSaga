using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardMage : CardBasicMob
    {
        public CardMage()
        {
            Name = "A Mage";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Followers/mage.jpg";

            CardFlags = CardFlagsEnum.Mage;
        }
    }
}
