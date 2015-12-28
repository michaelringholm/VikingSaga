using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardBarmaid : CardBasicMob
    {
        public CardBarmaid()
        {
            Name = "A Barmaid";
            ImageUrl = @"Data/Gfx/Cards/NPC/barmaid.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}
