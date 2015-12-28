﻿using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardSmallBear : CardBasicMob
    {
        public CardSmallBear()
        {
            Name = "A Small Bear";
            HpBase = 2;
            DmgBase = 1;
            ImageUrl = @"small-bear.png";
        }
    }
}
