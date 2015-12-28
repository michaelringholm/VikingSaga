using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardSmallFox : CardBasicMob
    {
        public CardSmallFox()
        {
            Name = "A Small Fox";
            HpBase = 20;
            DmgBase = 2;
            ImageUrl = @"small-fox.png";
            Description = "Stupid fox!";

            AddSpellProperty(SpellProperty.ProtectLeftAndRight("Protect"));
        }
    }
}
