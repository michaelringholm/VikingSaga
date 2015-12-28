using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardSkeletonWarrior : CardBasicMob
    {
        public CardSkeletonWarrior()
        {
            Name = "A Skeleton Warrior";
            HpBase = 1;
            DmgBase = 1;
            ImageUrl = @"skeleton-warrior.png";

            var prop = SpellProperty.Revive(true, "Rise again!");
            AddSpellProperty(prop);
        }
    }
}
