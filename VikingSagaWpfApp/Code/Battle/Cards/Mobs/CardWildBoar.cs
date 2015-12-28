using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
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
