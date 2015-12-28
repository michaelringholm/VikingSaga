using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardMerchant : CardBasicMob
    {
        public CardMerchant()
        {
            Name = "A Merchant";
            ImageUrl = @"Data/Gfx/Cards/NPC/merchant.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}
