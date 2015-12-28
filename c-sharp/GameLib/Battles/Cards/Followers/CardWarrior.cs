using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardWarrior : CardBasicMob
    {
        public CardWarrior()
        {
            Name = "A Warrior";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Followers/warrior.jpg";

            CardFlags = CardFlagsEnum.Warrior;
        }
    }
}
