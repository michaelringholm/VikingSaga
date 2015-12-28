using System;
using System.Collections.Generic;
using VikingSaga.Code;

namespace VikingSagaWpfApp.Code.Battle
{
    public class Player
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Mana { get; set; }
        public BattleDeck Deck { get; private set; }
        public Hand Hand { get; private set; }
        public List<Card> SacrificedCards { get; private set; }
        public List<Card> DefeatedCards { get; private set; }

        public Player()
        {
            Deck = new BattleDeck();
            Hand = new Hand();
        }

        public virtual void TakeTurn(Battle battle)
        {
            AiPlaceCardsSimple(battle);
        }

        public int TakeHit(Card card)
        {
            Hp -= card.Attack;
            return card.Attack;
        }

        public bool IsDead { get { return Hp <= 0; } }

        // Draw from deck until hand is full or deck is empty
        public virtual void DrawFromDeck()
        {
            while (true)
            {
                int handIdx;
                if (!Hand.TryGetFreePosition(out handIdx))
                    break;

                var card = Deck.DrawCard();
                if (card == null)
                    break;

                Hand.Cards[handIdx] = card;
                Battle.Observer.CardDrawn(this, handIdx);
            }
        }

        public void PlaceCard(Battle battle, Card card, int position, bool removeFromHand)
        {
            var row = battle.Board.GetRow(this);
            row.PlaceCard(card, position);

            if (removeFromHand)
            {
                if (Hand.Cards[position] == null)
                    throw new InvalidOperationException("Hand does not contain a card to remove. Position = " + position);

                Hand.Cards[position] = null;
            }

            Mana -= card.ManaCost;
        }

        public void AiPlaceCardsSimple(Battle battle)
        {
            var row = battle.Board.GetRow(this);
            while (true)
            {
                int rowPosition;
                if (!row.TryGetFreePosition(out rowPosition))
                    break;

                Card card;
                int handPosition;
                if (!Hand.TryRemoveFirstCard(out card, out handPosition))
                    break;

                PlaceCard(battle, card, rowPosition, false);

                Battle.Observer.CardPlacedByAi(this, handPosition, rowPosition);
            }
        }
    }
}
