using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardWorm : CardBasicMob
    {
        public CardWorm()
        {
            Name = "A Large Worm";
            HpBase = 5;
            DmgBase = 5;
            ImageUrl = @"worm.png";
        }
    }
}
