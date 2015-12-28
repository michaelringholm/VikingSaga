using System;
using System.Collections.Generic;
using System.Diagnostics;
using VikingSaga.Code;
using GameLib.Battles.Cards;

namespace GameLib.Battles
{
    public class BattleDeck
    {
        public List<CardBattle> Cards { get; private set; }

        public BattleDeck()
        {
            Cards = new List<CardBattle>();
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

        public static void Shuffle(List<CardBattle> cards)
        {
            var rnd = new Random(((object)cards).GetHashCode() + DateTime.Now.Millisecond);
            var tmp = new List<CardBattle>();

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

        public void SetCards(IEnumerable<CardBattle> newCards)
        {
            Cards.Clear();
            Cards.AddRange(newCards);

            Shuffle();
        }

        public CardBattle DrawCard()
        {
            if (Cards.Count == 0)
                return null;

            var result = Cards[0];
            Cards.RemoveAt(0);
            return result;
        }
    }
}
