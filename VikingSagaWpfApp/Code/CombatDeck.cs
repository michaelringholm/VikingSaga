using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public class CombatDeck
    {
        public CombatDeck()
        {
            ActiveCards = new Card[5];
            PlayableCards = new Card[5];
            SacrificedCards = new List<Card>();
            DefeatedCards = new List<Card>();
        }

        public List<Card> RemainingCards { get; set; }
        public List<Card> SacrificedCards { get; set; }
        public List<Card> DefeatedCards { get; set; }
        public Card[] ActiveCards { get; set; }
        public Card[] PlayableCards { get; set; }

        public int GetNextEmptyActiveCardIndex()
        {
            for (int i = 0; i < 5; i++)
            {
                if (ActiveCards[i] == null)
                    return i;
            }

            throw new Exception("No empty card indexes left");
        }

        public int GetNextEmptyPlayableCardIndex()
        {
            for (int i = 0; i < 5; i++)
            {
                if (PlayableCards[i] == null)
                    return i;
            }

            throw new Exception("No empty card indexes left");
        }



        internal Card.ConditionEnum TakeHit(int cardIndex, Card attackingCard)
        {
            var card = ActiveCards[cardIndex];
            card.TakeHit(attackingCard);
            var condition = card.Condition;

            if (condition == Card.ConditionEnum.Defeated)
            {
                DefeatedCards.Add(card);
                ActiveCards[cardIndex] = null;
            }

            return condition;
        }
    }
}
