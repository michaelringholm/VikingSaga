using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardTroll : CardBasicMob
    {
        public CardTroll()
        {
            Name = "A Troll";
            HpBase = 4;
            DmgBase = 3;
            ImageUrl = @"troll.png";
        }
    }
}
