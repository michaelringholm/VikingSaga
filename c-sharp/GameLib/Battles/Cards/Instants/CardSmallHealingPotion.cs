using System;

namespace GameLib.Battles.Cards
{
    class CardSmallHealingPotion : CardInstantHpChange
    {
        public CardSmallHealingPotion()
        {
            Name = "A small healing potion";
            Amount = 2;
            ImageUrl = "healing-potion.png";
            AmountType = HpChangeTypeEnum.Physical;
            Power = 1;

            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;
        }
    }
}
