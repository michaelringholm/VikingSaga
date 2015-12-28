using System;

namespace GameLib.Battles.Cards
{
    class CardNarkrall : CardBasicMob
    {
        public CardNarkrall()
        {
            Name = "Narkrall Rakeclaw";
            HpBase = 10;
            DmgBase = 5;
            ImageUrl = @"Data/Gfx/Cards/Bosses/narkrall.jpg";
            Rarity = CardRarity.Rare;
        }
    }
}
