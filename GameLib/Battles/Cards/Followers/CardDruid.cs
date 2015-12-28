using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardDruid : CardBasicMob
    {
        public CardDruid()
        {
            Name = "A Druid";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Followers/druid.jpg";

            CardFlags = CardFlagsEnum.Druid;
        }
    }
}
