using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardWolfPet : CardBasicMob
    {
        public CardWolfPet()
        {
            Name = "Ghost";
            HpBase = 6;
            DmgBase = 3;
            ImageUrl = @"Data/Gfx/Cards/Mobs/wolf.jpg";
            Rarity = CardRarity.Epic;

            var prop = SpellProperty.Rage("<Roars>", 3);
            AddSpellProperty(prop);

            CardFlags = CardFlagsEnum.Minion;
        }
    }
}
