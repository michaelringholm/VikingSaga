using System;

namespace GameLib.Battles.Cards
{
    class CardSmallRabbit : CardBasicMob
    {
        public CardSmallRabbit()
        {
            Name = "A Small Rabbit";
            HpBase = 4;
            DmgBase = 1;
            ImageUrl = "small-rabbit.png";

            Description = "A small furry rabbit. Nothing to be worried about. Testing the amount of text that can fit inside a card without losing any and so on so it must be a long text!";

            //AddSpellProperty(SpellProperty.DoubleAttack());
            //AddSpellProperty(SpellProperty.Charge(string.Empty));

        }
    }
}
