using System;
using System.Collections.Generic;
using VikingSaga.Code;
using GameLib.Battles.Cards;

namespace GameLib.Battles
{
    public class Hand
    {
        public CardBattle[] Cards { get; private set; }

        public Hand()
        {
            Cards = new CardBattle[Board.CardsPerRow];
        }

        public IEnumerable<CardBattle> AllCards()
        {
            foreach (CardBattle card in Cards)
            {
                if (card != null)
                    yield return card;
            }
        }

        public void CopyFrom(Hand other, Player newOwner)
        {
            Array.Clear(this.Cards, 0, this.Cards.Length);

            for (int i = 0; i < other.Cards.Length; ++i)
            {
                var card = other.Cards[i];
                if (card != null)
                {
                    var clone = card.Clone();
                    clone.Owner = newOwner;
                    clone.Observer = newOwner.Observer;
                    Cards[i] = clone;
                }
            }
        }

        public bool TryGetFreePosition(out int position)
        {
            return BattleUtil.TryGetFirstNull(Cards, out position);
        }

        public bool TryGetFirstCardPosition(out int position)
        {
            return BattleUtil.TryGetFirstNotNull(Cards, out position);
        }

        public bool TryRemoveFirstCard(out CardBattle card, out int position)
        {
            card = null;

            if (!TryGetFirstCardPosition(out position))
                return false;

            card = Cards[position];
            Cards[position] = null;

            return true;
        }
    }
}
