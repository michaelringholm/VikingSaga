using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardBandit : CardBasicMob
    {
        public CardBandit()
        {
            Name = "A Bandit";
            HpBase = 4;
            DmgBase = 5;
            ImageUrl = @"bandit.png";
        }
    }
}
