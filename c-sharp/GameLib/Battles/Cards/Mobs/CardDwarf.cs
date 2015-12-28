using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardDwarf : CardBasicMob
    {
        public CardDwarf()
        {
            Name = "A Dwarf";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Mobs/dwarf.jpg";
            CardFlags = CardFlagsEnum.Warrior;
        }
    }
}
