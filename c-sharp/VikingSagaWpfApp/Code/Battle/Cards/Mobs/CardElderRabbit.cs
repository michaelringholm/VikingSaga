using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardElderRabbit : CardBasicMob
    {
        public CardElderRabbit()
        {
            Name = "An Elder Rabbit";
            HpBase = 8;
            DmgBase = 1;
            ImageUrl = "elder-rabbit.png";

            AddSpellProperty(SpellProperty.ProtectLeftAndRight("Saved!"));

            Description = "A small diseased rabbit. Nothing to be worried about. Testing the amount of text that can fit inside a card without losing any and so on so it must be a long text!";

        }
    }
}
