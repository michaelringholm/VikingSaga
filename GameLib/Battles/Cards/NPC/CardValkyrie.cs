﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class CardValkyrie : CardBasicMob
    {
        public CardValkyrie()
        {
            Name = "A Valkyrie";
            ImageUrl = @"Data/Gfx/Cards/NPC/valkyrie.jpg";

            CardFlags = CardFlagsEnum.NPC;
        }
    }
}