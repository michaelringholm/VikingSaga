using System;
using System.Collections.Generic;
using VikingSaga.Code;
using GameLib.Battles.Cards;

namespace GameLib.Battles
{
    public abstract class Player
    {
        public IBattleObserver Observer { get; set; }
        public string Name { get; set; }
        public int Turn { get; set; }
        public int Hp { get; set; }
        public int Mana { get; set; }
        public BattleDeck Deck { get; private set; }
        public CardBattle[] Party { get; set; }
        public Hand Hand { get; private set; }
        public List<CardBattle> DefeatedCards { get; private set; }
        public Battle Battle { get; set; }
        public bool[] TurnTakenFlags { get; private set; }

        public List<string> ScoreDebug = new List<string>();

        private float _score;
        public float Score { get { return _score; } }
        public void UpdateScore(float amount, string reason)
        {
            _score += amount;
            ScoreDebug.Add(string.Format("{0} : {1:0.0}", reason, amount));
        }

        public Player()
        {
            Deck = new BattleDeck();
            Hand = new Hand();
            Party = new CardBattle[5];
            DefeatedCards = new List<CardBattle>();
            TurnTakenFlags = new bool[5];
        }

        public void ResetTurnTakenFlags()
        {
            for (int i = 0; i < TurnTakenFlags.Length; ++i)
                TurnTakenFlags[i] = false;
        }

        public abstract void TakeTurn();

        public int HpChange(int amount)
        {
            Hp += amount;
            UpdateScore(amount * 4, "Hp change (player)");

            if (IsDead)
                UpdateScore(-100000, "Self died");

            return amount;
        }

        public int ManaChange(int amount)
        {
            Mana += amount;
            UpdateScore(amount, "Mana change (player)");
            return amount;
        }

        public bool IsDead { get { return Hp <= 0; } }

        public virtual void PlacePartyCards()
        {
            if (Party == null)
                return;

            for (int i = 0; i < 5; ++i)
            {
                var card = Party[i];
                if (card != null)
                {
                    card.Owner = this;
                    Observer.PartyCardPlaced(this, i);
                }
            }
        }

        public virtual void DrawFromDeck(int max = int.MaxValue)
        {
            int count = 0;
            while (true)
            {
                if (count >= max)
                    break;

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

                count++;
            }
        }

        private void ClearHandPosition(CardBattle card)
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

            Mana -= card.Power;

            UpdateScore(1, "Dropped on board");
            if (card.HasAnySpellProperty())
            {
                UpdateScore(1, "Dropped has SpellProp");
            }

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
                UpdateScore(card.Dmg, "Charge");
            }
        }

        public void DropCardOtherCard(CardInstant src, CardBasicMob dst, bool isAi)
        {
            ClearHandPosition(src);

            Mana -= src.Power;

            if (src is CardInstantHpChange)
            {
                var c = (CardInstantHpChange)src;
                int amount = dst.ChangeHp(c.Amount, c.AmountType);
                float score = amount * (dst.Owner == this ? 1 : -1);
                UpdateScore(amount, c.Name + " on " + dst.Name);

                if (dst.IsDead)
                {
                    src.BattleStatistics.KillingBlowsOnEnemyCards++;
                    UpdateScore(dst.Dmg, c.Name + " on " + dst.Name + " (kill bonus)");
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

                float situationalBonus = c.Property.SituationalBonus(dst);
                if (situationalBonus != 0.0f)
                {
                    UpdateScore(situationalBonus, c.Name + " on " + dst.Name + " (Prop score)");
                }

                dst.AddSpellProperty(c.Property);

                float score = 5 * (c.Effect == SpellProperty.Result.Positive ? 1 : -1) * (dst.Owner == this ? 1 : -1);
                UpdateScore(score, c.Name + " on " + dst.Name);

                Observer.DropCardOnCard(src, dst, 0, isAi);
            }
            else if (src is CardInstantDmgChange)
            {
                var c = (CardInstantDmgChange)src;
                dst.ChangeDmgSpell(c.Amount);
                float score = c.Amount * (dst.Owner == this ? 1 : -1);
                UpdateScore(score, c.Name + " on " + dst.Name);
                Observer.DropCardOnCard(src, dst, c.Amount, isAi);
            }
            else if (src is CardInstantCustom)
            {
                var c = (CardInstantCustom)src;
                c.Execute();
                Observer.DropCardOnCard(src, dst, 0, isAi);
                float score = 5 * (c.Effect == SpellProperty.Result.Positive ? 1 : -1) * (dst.Owner == this ? 1 : -1);
                UpdateScore(score, c.Name + " on " + dst.Name);
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
    }
}
