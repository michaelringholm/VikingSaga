using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardRaven : CardBasicMob
    {
        public CardRaven()
        {
            Name = "A Raven";
            HpBase = 3;
            DmgBase = 5;
            ImageUrl = @"raven.png";
        }
    }
}
