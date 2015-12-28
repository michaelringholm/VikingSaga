using System;
using System.Collections.Generic;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSagaWpfApp.Code.Battle
{
    public abstract class Player
    {
        public IBattleObserver Observer { get; set; }
        public string Name { get; set; }
        public int Turn { get; set; }
        public int Hp { get; set; }
        public int Mana { get; set; }
        public BattleDeck Deck { get; private set; }
        public Hand Hand { get; private set; }
        public List<BattleCard> DefeatedCards { get; private set; }
        public Battle Battle { get; set; }

        public Player()
        {
            Deck = new BattleDeck();
            Hand = new Hand();
            DefeatedCards = new List<BattleCard>();
        }

        public abstract void TakeTurn(Battle battle);

        public int HpChange(int amount)
        {
            Log.Line(string.Format("Player {0} + Hp : {1}", this.Name, amount));
            Hp += amount;

            return amount;
        }

        public int ManaChange(int amount)
        {
            Log.Line(string.Format("Player {0} + mana : {1}", this.Name, amount));
            Mana += amount;
            return amount;
        }

        public bool IsDead { get { return Hp <= 0; } }

        // Draw from deck until hand is full or deck is empty
        public virtual void DrawFromDeck()
        {
            while (true)
            {
                int handPosition;
                if (!Hand.TryGetFreePosition(out handPosition))
                    break;

                var card = Deck.DrawCard();
                if (card == null)
                    break;

                card.HandPosition = handPosition;
                card.Owner = this;
                card.BattleStatistics.WasInHand = true;
                Hand.Cards[handPosition] = card;

                Observer.CardDrawn(this, handPosition);
            }
        }

        private void ClearHandPosition(BattleCard card)
        {
            if (Hand.Cards[card.HandPosition] == null)
                throw new InvalidOperationException("Hand does not contain a card to remove. Position = " + card.HandPosition);

            Hand.Cards[card.HandPosition] = null;
            card.HandPosition = -1;
        }

        public void DropCardOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi)
        {
            var row = Battle.Board.GetRow(this);
            row.PlaceCard((CardBasicMob)card, boardPosition);
            card.BoardPosition = boardPosition;
            card.BattleStatistics.WasOnBoard = true;

            ClearHandPosition(card);

            Mana -= card.Mana;

            Observer.CardPlacedOnBoard(card, handPosition, boardPosition, isAi);
            card.OnBattleFlowEvent(BattleFlowEvent.AfterEnterBoard);

            SpellProperty prop;
            if (card.TryGetSpellProperty(SpellPropertyType.Charge, out prop))
            {
                var mobCard = (CardBasicMob)card;
                mobCard.AddMainUiSpellOutput(prop);
                mobCard.RemoveSpellProperty(prop);

                Observer.ShowNotifications();
                Battle.OutOfOrderCardTurn(this, card.BoardPosition);
            }
        }

        public void DropCardOtherCard(CardInstant src, CardBasicMob dst, bool isAi)
        {
            ClearHandPosition(src);

            if (src is CardInstantHpChange)
            {
                var c = (CardInstantHpChange)src;
                int amount = dst.ChangeHp(c.Amount, c.AmountType);
                if (dst.IsDead)
                {
                    src.BattleStatistics.KillingBlowsOnEnemyCards++;
                }

                Observer.DropCardOnCard(src, dst, amount, isAi);

                if (dst.IsDead)
                {
                    Battle.CardDied(dst);
                }
            }
            else if (src is CardInstantSpellProperty)
            {
                var c = (CardInstantSpellProperty)src;
                dst.AddSpellProperty(c.Property);
                Observer.DropCardOnCard(src, dst, 0, isAi);
            }
            else if (src is CardInstantDmgChange)
            {
                var c = (CardInstantDmgChange)src;
                dst.ChangeDmgSpell(c.Amount);
                Observer.DropCardOnCard(src, dst, c.Amount, isAi);
            }
            else if (src is CardInstantCustom)
            {
                var c = (CardInstantCustom)src;
                c.Execute();
                Observer.DropCardOnCard(src, dst, 0, isAi);
                int count = Battle.HandleDeadCards();
                if (count > 0)
                {
                    Observer.RemoveDeadCardsOnBoard();
                }
            }
            else if (src is CardInstantManaChange)
            {
                throw new InvalidOperationException("Cannot cast mana on card");
            }
            else throw new NotImplementedException("Unknown card type");
        }

        public void DropCardOnPlayer(Player dst, CardInstant card, bool isAi)
        {
            ClearHandPosition(card);

            Mana -= card.Mana;

            if (card is CardInstantHpChange)
            {
                var c = (CardInstantHpChange)card;
                int amount = dst.HpChange(c.Amount);
                Observer.DropCardOnPlayer(dst, card, amount, isAi);
            }
            else if (card is CardInstantManaChange)
            {
                var c = (CardInstantManaChange)card;
                int amount = dst.ManaChange(c.Amount);
                Observer.DropCardOnPlayer(dst, card, amount, isAi);
            }
            else if (card is CardInstantCustom)
            {
                var c = (CardInstantCustom)card;
                c.Execute();
                Observer.DropCardOnPlayer(dst, card, 0, isAi);
                int count = Battle.HandleDeadCards();
                if (count > 0)
                {
                    Observer.RemoveDeadCardsOnBoard();
                }
            }
            else throw new NotImplementedException("Unknown card type");
        }
    }
}
