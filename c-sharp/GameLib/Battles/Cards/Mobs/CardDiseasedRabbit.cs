using System;

namespace GameLib.Battles.Cards
{
    class CardDiseasedRabbit : CardBasicMob
    {
        public CardDiseasedRabbit()
        {
            Name = "A Diseased Rabbit";
            HpBase = 4;
            DmgBase = 1;
            ImageUrl = "diseased-rabbit.png";

            AddSpellProperty(SpellProperty.Poison(1, 2, "Bite!", "Ouch!", "Ugh!"));

            Description = "A small diseased rabbit. Nothing to be worried about. Testing the amount of text that can fit inside a card without losing any and so on so it must be a long text!";

        }
    }
}
