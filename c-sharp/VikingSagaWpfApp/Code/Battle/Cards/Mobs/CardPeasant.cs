using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardPeasant : CardBasicMob
    {
        public CardPeasant()
        {
            Name = "A Peasant";
            HpBase = 5;
            DmgBase = 1;
            ImageUrl = "peasant.png";

            var prop = SpellProperty.Revive(false, "<Quaffs a potion>");
            AddSpellProperty(prop);
        }
    }
}
