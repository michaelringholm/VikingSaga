using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardSmallManaPotion : CardInstantManaChange
    {
        public CardSmallManaPotion()
        {
            Name = "A small mana potion";
            Amount = 5;
            ImageUrl = "mana-potion.png";
            Mana = 0;

            CanTargetOwnPlayer = true;
            CanTargetEnemyPlayer = true;
        }
    }
}
