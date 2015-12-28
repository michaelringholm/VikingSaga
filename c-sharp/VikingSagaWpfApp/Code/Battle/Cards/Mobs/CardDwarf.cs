using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardDwarf : CardBasicMob
    {
        public CardDwarf()
        {
            Name = "A Dwarf";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"dwarf.png";
        }
    }
}
