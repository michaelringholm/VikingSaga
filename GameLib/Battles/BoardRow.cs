using System;
using System.Collections.Generic;
using VikingSaga.Code;
using GameLib.Battles.Cards;

namespace GameLib.Battles
{
    public class BoardRow
    {
        public BoardRow()
        {
            _cards = new CardBasicMob[Board.CardsPerRow];
            Cards = (IReadOnlyList<CardBasicMob>)_cards;
        }

        public void CopyFrom(BoardRow other, Player newOwner)
        {
            Array.Clear(_cards, 0, _cards.Length);

            for (int i = 0; i < other._cards.Length; ++i)
            {
                var card = other._cards[i];
                if (card != null)
                {
                    var clone = (CardBasicMob)card.Clone();
                    clone.Owner = newOwner;
                    clone.Observer = newOwner.Observer;
                    _cards[i] = clone;
                }
            }
        }

        public bool TryGetFreePosition(out int position)
        {
            return BattleUtil.TryGetFirstNull(_cards, out position);
        }

        public void PlaceCard(CardBasicMob card, int position)
        {
            if (_cards[position] != null)
                throw new ArgumentException("Already a card at position " + position);

            _cards[position] = card;
        }

        public CardBattle RemoveCard(int position)
        {
            var card = _cards[position];
            _cards[position] = null;
            return card;
        }

        public IEnumerable<CardBasicMob> AllCards()
        {
            foreach (CardBasicMob card in Cards)
            {
                if (card != null)
                    yield return card;
            }
        }

        private CardBasicMob[] _cards;
        public IReadOnlyList<CardBasicMob> Cards { get; private set; }
    }
}
