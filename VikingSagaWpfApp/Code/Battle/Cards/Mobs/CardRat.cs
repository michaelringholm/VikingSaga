using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardRat : CardBasicMob
    {
        public CardRat()
        {
            Name = "A Rat";
            HpBase = 4;
            DmgBase = 2;
            ImageUrl = @"rat.png";

            var prop = SpellProperty.Rage("<Roars>", 3);
            AddSpellProperty(prop);
        }
    }
}
