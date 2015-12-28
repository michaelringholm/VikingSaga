using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardWolfPet : CardBasicMob
    {
        public CardWolfPet()
        {
            Name = "Ghost";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"wolf.png";

            var prop = SpellProperty.Rage("<Roars>", 3);
            AddSpellProperty(prop);
        }
    }
}
