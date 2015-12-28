using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardShadow : CardBasicMob
    {
        public CardShadow()
        {
            Name = "A Shadow";
            HpBase = 5;
            DmgBase = 5;
            ImageUrl = @"shadow.png";
        }
    }
}
