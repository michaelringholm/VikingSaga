using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardDungeonMaster : CardBasicMob
    {
        public CardDungeonMaster()
        {
            Name = "A Dungeon Master";
            ImageUrl = @"Data/Gfx/Cards/NPC/dungeon-master.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}
