using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardSmallHealingPotion : CardInstantHpChange
    {
        public CardSmallHealingPotion()
        {
            Name = "A small healing potion";
            Amount = 2;
            ImageUrl = "healing-potion.png";
            AmountType = HpChangeType.Physical;
            Mana = 1;

            CanTargetOwnCard = true;
            CanTargetOwnPlayer = true;
            CanTargetEnemyPlayer = true;
            CanTargetEnemyCard = true;
        }
    }
}
