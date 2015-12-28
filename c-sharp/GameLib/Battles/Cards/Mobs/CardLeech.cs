using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardLeech : CardBasicMob
    {
        public CardLeech()
        {
            Name = "A Leech";
            HpBase = 5;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Mobs/leech.jpg";
            CardFlags = CardFlagsEnum.Minion;
        }
    }
}
