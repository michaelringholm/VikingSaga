﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardAbility : CardBattle
    {
        public int PowerCost { get; protected set; }

        public CardAbility()
        {
            PowerCost = 1;
        }
    }
}