using System;

namespace GameLib.Battles.Cards
{
    public class CardFireball : CardInstantHpChange
    {
        public CardFireball()
        {
            Name = "Fireball";
            Amount = -1;
            AmountType = HpChangeTypeEnum.Fire;

            CanTargetEnemyCard = true;
            CanTargetOwnCard = true;
        }
    }
}
