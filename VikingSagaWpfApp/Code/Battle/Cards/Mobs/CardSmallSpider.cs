using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardSmallSpider : CardBasicMob
    {
        public CardSmallSpider()
        {
            Name = "A Small Spider";
            HpBase = 3;
            DmgBase = 2;
            ImageUrl = @"spider-small.png";

            var prop = SpellProperty.Poison(1, 2, string.Empty, "<Poisened>", "<Poison>");
            AddSpellProperty(prop);
        }
    }
}
