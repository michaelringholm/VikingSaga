using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardHunter : CardBasicMob
    {
        public CardHunter()
        {
            Name = "A Hunter";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Followers/hunter.jpg";

            CardFlags = CardFlagsEnum.Hunter;
        }
    }
}
