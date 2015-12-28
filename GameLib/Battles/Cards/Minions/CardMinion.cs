using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardMinion : CardBasicMob
    {
        public CardMinion()
        {
            Name = "A minion";
            HpBase = 3;
            DmgBase = 1;
            ImageUrl = @"Data/Gfx/Cards/Minions/minion.jpg";

            CardFlags = CardFlagsEnum.Minion;            
        }
    }
}
