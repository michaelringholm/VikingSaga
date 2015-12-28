using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardWildBoar : CardBasicMob
    {
        public CardWildBoar()
        {
            Name = "A Wild Boar";
            HpBase = 1;
            DmgBase = 1;
            ImageUrl = @"wild-boar.png";
        }
    }
}
