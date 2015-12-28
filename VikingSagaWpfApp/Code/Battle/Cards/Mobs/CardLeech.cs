using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardLeech : CardBasicMob
    {
        public CardLeech()
        {
            Name = "A Leech";
            HpBase = 5;
            DmgBase = 3;
            ImageUrl = @"leech.png";
        }
    }
}
