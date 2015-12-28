using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardGhoul : CardBasicMob
    {
        public CardGhoul()
        {
            Name = "A Ghoul";
            HpBase = 3;
            DmgBase = 3;
        }
    }
}
