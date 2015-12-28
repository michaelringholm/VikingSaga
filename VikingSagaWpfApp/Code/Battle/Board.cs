using System.Linq;
using System.Collections.Generic;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class Board
    {
        public static readonly int CardsPerRow = 5;

        public Board(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            RowPlayer1 = new BoardRow();
            RowPlayer2 = new BoardRow();
        }

        public void CopyFrom(Board other)
        {
            this.RowPlayer1.CopyFrom(other.RowPlayer1, Player1);
            this.RowPlayer2.CopyFrom(other.RowPlayer2, Player2);
        }

        public IEnumerable<CardBasicMob> AllCards(Player player)
        {
            return GetRow(player).Cards.Where((c) => c != null);
        }

        public IEnumerable<CardBasicMob> AllCards()
        {
            return AllCards(Player1).Concat(AllCards(Player2));
        }

        public CardBasicMob GetCardAt(Player player, int position)
        {
            if (position < 0 || position > 5)
                return null;

            var row = GetRow(player);
            return row.Cards[position];
        }

        public BoardRow GetRow(Player player)
        {
            return player == Player1 ? RowPlayer1 : RowPlayer2;
        }

        public CardBasicMob GetCard(Player player, int position)
        {
            var row = GetRow(player);
            return row.Cards[position];
        }

        public void RemoveCard(CardBasicMob card)
        {
            RemoveCard(card.Owner, card.BoardPosition);
        }

        public void RemoveCard(Player player, int position)
        {
            var row = GetRow(player);
            row.RemoveCard(position);
        }

        private Player Player1 { get; set; }
        private Player Player2 { get; set; }

        public BoardRow RowPlayer1 { get; private set; }
        public BoardRow RowPlayer2 { get; private set; }
    }
}
