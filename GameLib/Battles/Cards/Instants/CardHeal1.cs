using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardHeal1 : CardInstantHpChange
    {
        public CardHeal1()
        {
            Name = "Heal 1";
            Description = "This card will heal any of your minions or followers by 1 HP.";
            Amount = 1;
            AmountType = HpChangeTypeEnum.Magic;
            ImageUrl = "Data/Gfx/Cards/Abilities/healing-potion.jpg";

            CanTargetEnemyCard = true;
            CanTargetOwnCard = true;

            CardFlags = CardFlagsEnum.Heal;
        }

    }
}
