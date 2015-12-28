using System;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    class CardWarcry : CardInstantCustom
    {
        public CardWarcry()
        {
            Name = "Hero: Warcry";
            ExecuteAction = DoWarcry;

            CanTargetOwnPlayer = true;
            Effect = SpellProperty.Result.Positive;
            ImageUrl = "warcry.png";
        }

        private void DoWarcry()
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
