using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardGhost : CardBasicMob
    {
        public CardGhost()
        {
            Name = "A Ghost";
            HpBase = 4;
            DmgBase = 4;
            ImageUrl = @"ghost.png";            
        }
    }
}
