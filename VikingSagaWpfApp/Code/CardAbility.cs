using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code
{
    public class CardAbility
    {
        public enum CardAbilityEnum 
        { 
            Poison1, // Poision1 deals 20% poison damage based on attack skill for 3 rounds, event if card is defeated
            Bleed1, // Bleed deals 20% bleed damage based on attack skill for 3 rounds, event if card is defeated
            LifeTap1, // On attack the attacking card regains 30% HP of attack skill
            Snare1, // Snares target for 2 rounds event if card is defeated, card cant reach enemy hero
            Rabies1, // Opponent card looses 1 attack skill when affected by rabies, lasts for 1 round
            Dodge1, // Card avoids one damage when attacked by melee
        }
    }
}
