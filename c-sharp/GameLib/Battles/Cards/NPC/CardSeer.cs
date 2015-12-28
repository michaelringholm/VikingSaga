using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardSeer : CardBasicMob
    {
        public CardSeer()
        {
            Name = "A Seer";
            ImageUrl = @"Data/Gfx/Cards/NPC/seer.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}
