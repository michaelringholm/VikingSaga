using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    class CardStrength : CardInstantDmgChange
    {
        public CardStrength()
        {
            Name = "Strength";
            Description = "This card will grant the follower or minion a +1 bonus to all melee attacks.";
            ImageUrl = "Data/Gfx/Cards/Abilities/strength.jpg";
            CanTargetOwnCard = true;
            CanTargetEnemyCard = true;

            OnApplyMsg = "+1 dmg";

            Amount = 1;

            CardFlags = CardFlagsEnum.Buff;
        }
    }
}
