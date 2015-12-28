using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardPhoenix : CardBasicMob
    {
        public CardPhoenix()
        {
            Name = "A Phoenix";
            HpBase = 4;
            DmgBase = 3;

            var prop = SpellProperty.Revive(true, "Rise again!");
            AddSpellProperty(prop);
        }
    }
}
