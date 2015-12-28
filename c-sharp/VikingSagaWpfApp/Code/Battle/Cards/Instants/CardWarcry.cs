using System;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    class CardWarcry : CardInstantCustom
    {
        public CardWarcry()
        {
            Name = "Hero: Warcry";

            CanTargetOwnPlayer = true;
            ImageUrl = "warcry.png";
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
