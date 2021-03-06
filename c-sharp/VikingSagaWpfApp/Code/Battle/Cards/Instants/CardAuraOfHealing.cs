﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardAuraOfHealing : CardInstantCustom
    {
        public CardAuraOfHealing()
        {
            Name = "Hero: Aura of Healing";

            CanTargetOwnPlayer = true;
        }

        public override SpellProperty.Result Effect { get { return SpellProperty.Result.Positive; } }

        public override void Execute()
        {
            var cards = this.Owner.Battle.Board.AllCards(this.Owner);
            foreach (var card in cards)
            {
                card.AddUiSpellOutput("<AoH>");
                card.AddSpellProperty(SpellProperty.Regen(1, "<AoH>"));
                card.ChangeHp(1, HpChangeType.Magic);
            }

            Observer.ShowNotifications();
        }
    }
}
