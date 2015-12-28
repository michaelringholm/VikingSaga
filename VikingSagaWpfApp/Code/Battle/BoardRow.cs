using System;
using System.Collections.Generic;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSagaWpfApp.Code.Battle
{
    public class BoardRow
    {
        public BoardRow()
        {
            _cards = new CardBasicMob[Board.CardsPerRow];
            Cards = (IReadOnlyList<CardBasicMob>)_cards;
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

        public BattleCard RemoveCard(int position)
        {
            var card = _cards[position];
            _cards[position] = null;
            return card;
        }

        private CardBasicMob[] _cards;
        public IReadOnlyList<CardBasicMob> Cards { get; private set; }
    }
}
