using System;

namespace GameLib.Battles.Cards
{
    class CardWarcry : CardInstantCustom
    {
        public CardWarcry()
        {
            Name = "Warcry";
            ImageUrl = "Data/Gfx/Cards/Abilities/warcry.png";
            CardFlags = CardFlagsEnum.Buff;
        }

        public override SpellProperty.Result Effect { get { return SpellProperty.Result.Positive; } }

        public override void Execute()
        {
            var cards = this.Owner.Battle.Board.AllCards(this.Owner);
            foreach(var card in cards)
            {
                card.AddUiSpellOutput("+1 dmg");
                card.ChangeDmgSpell(1);
            }

            Observer.ShowNotifications();
        }
    }
}
