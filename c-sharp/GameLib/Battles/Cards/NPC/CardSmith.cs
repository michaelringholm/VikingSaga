using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardSmith : CardBasicMob
    {
        public CardSmith()
        {
            Name = "A Smith";
            ImageUrl = @"Data/Gfx/Cards/NPC/smith.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}
