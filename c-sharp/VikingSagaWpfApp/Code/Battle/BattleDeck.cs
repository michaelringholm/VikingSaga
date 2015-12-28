using System;
using System.Collections.Generic;
using System.Diagnostics;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class BattleDeck
    {
        public List<BattleCard> Cards { get; private set; }

        public BattleDeck()
        {
            Cards = new List<BattleCard>();
        }

        public void CopyFrom(BattleDeck other)
        {
            Cards.Clear();
            foreach(var card in other.Cards)
            {
                var clone = card.Clone();
                Cards.Add(clone);
            }
        }

        public static void Shuffle(List<BattleCard> cards)
        {
            var rnd = new Random(((object)cards).GetHashCode() + DateTime.Now.Millisecond);
            var tmp = new List<BattleCard>();

            int totalCardCount = cards.Count;
            for (int i = 0; i < totalCardCount; ++i)
            {
                int pickIdx = rnd.Next(cards.Count);
                var pick = cards[pickIdx];
                tmp.Add(pick);
                cards.RemoveAt(pickIdx);
            }

            Debug.Assert(cards.Count == 0);
            cards.AddRange(tmp);
        }

        public void Shuffle()
        {
            BattleDeck.Shuffle(Cards);
        }

        public void SetCards(IEnumerable<BattleCard> newCards)
        {
            Cards.Clear();
            Cards.AddRange(newCards);

            Shuffle();
        }

        public BattleCard DrawCard()
        {
            if (Cards.Count == 0)
                return null;

            var result = Cards[0];
            Cards.RemoveAt(0);
            return result;
        }
    }
}
