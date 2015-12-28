using System;

namespace GameLib.Battles.Cards
{
    class CardPixie : CardBasicMob
    {
        public CardPixie()
        {
            Name = "A Pixie";
            HpBase = 3;
            DmgBase = 1;
            ImageUrl = @"pixie-female-nut.png";

            var prop = SpellProperty.Regen(1, "Regen");
            AddSpellProperty(prop);
        }
    }
}
