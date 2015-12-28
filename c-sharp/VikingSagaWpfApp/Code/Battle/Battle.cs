using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class Battle
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }

        public Board Board { get; private set; }
        public int Round { get; private set; }

        private IBattleObserver Observer { get; set; }

        public Battle(Player player1, Player player2, IBattleObserver observer)
        {
            Player1 = player1;
            Player2 = player2;
            Player1.Battle = this;
            Player2.Battle = this;

            Observer = observer;
            Board = new Board(player1, player2);

            SetPlayerObserver(observer);
            SetObserverAllCards(observer, true);
        }

        public void CopyFrom(Battle other)
        {
            this.Round = other.Round;
            this.Board.CopyFrom(other.Board);

            this.FirstPlayer = other.IsPlayer1(other.FirstPlayer) ? this.Player1 : this.Player2;
            this.SecondPlayer = other.IsPlayer1(other.FirstPlayer) ? this.Player2 : this.Player1;
        }

        private void SetPlayerObserver(IBattleObserver observer)
        {
            Player1.Observer = observer;
            Player2.Observer = observer;
        }

        private bool WinnerFound()
        {
            return Player1.IsDead || Player2.IsDead;
        }

        public bool IsPlayer1(Player player)
        {
            return player == Player1;
        }

        public bool IsPlayer2(Player player)
        {
            return !IsPlayer1(player);
        }

        public Player GetOpponent(Player player)
        {
            return IsPlayer1(player) ? Player2 : Player1;
        }

        public void StartAsync()
        {
            Task.Run(() => Start());
        }

        private void RollForStartingplayer()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            double roll = rnd.NextDouble();
            FirstPlayer = roll < 0.5 ? Player1 : Player2;
            SecondPlayer = roll < 0.5 ? Player2 : Player1;

            FirstPlayer = Player1;
            SecondPlayer = Player2;
        }

        public void SetObserverAllCards(IBattleObserver observer, bool doInit = false)
        {
            var deckCards = Player1.Deck.Cards.Concat(Player2.Deck.Cards);
            var boardCards = Board.AllCards();
            var handCards = Player1.Hand.AllCards();
            var allCards = deckCards.Concat(boardCards).Concat(handCards);

            foreach (var card in allCards)
            {
                card.Observer = observer;
                if (doInit)
                {
                    card.Init();
                }
            }
        }

        public void Start()
        {
            RollForStartingplayer();

            Log.Line(string.Format("{0} is first, {1} is second", FirstPlayer.Name, SecondPlayer.Name));

            Observer.BattleStart(FirstPlayer);

            Round = 1;
            while (true)
            {
                DoRound(FirstPlayer, SecondPlayer);

                if (WinnerFound())
                    break;
            }

            Observer.BattleEnded(Player1.IsDead ? Player2 : Player1, Player1.IsDead ? Player1 : Player2);
        }

        private void RoundPreBattle(Player firstPlayer, Player secondPlayer)
        {
            firstPlayer.DrawFromDeck();
            secondPlayer.DrawFromDeck();

            SendBattleFlowEvent(firstPlayer, BattleFlowEvent.BeforeRound);
            SendBattleFlowEvent(secondPlayer, BattleFlowEvent.BeforeRound);

            PlayerTurn(firstPlayer);
            PlayerTurn(secondPlayer);
        }

        public void RoundPostBattle(Player firstPlayer, Player secondPlayer)
        {
            AddRoundMana(Player1, Round);
            AddRoundMana(Player2, Round);

            SendBattleFlowEvent(firstPlayer, BattleFlowEvent.AfterRound);
            SendBattleFlowEvent(secondPlayer, BattleFlowEvent.AfterRound);

            Round++;
        }

        public void DoCombat(Player firstPlayer, Player secondPlayer)
        {
            for (int i = 0; i < Board.CardsPerRow; ++i)
            {
                BoardCardTurn(firstPlayer, secondPlayer, i);
                if (WinnerFound())
                    return;
            }

            for (int i = 0; i < Board.CardsPerRow; ++i)
            {
                BoardCardTurn(secondPlayer, firstPlayer, i);
                if (WinnerFound())
                    return;
            }
        }

        private void DoRound(Player firstPlayer, Player secondPlayer)
        {
            RoundPreBattle(firstPlayer, secondPlayer);

            DoCombat(firstPlayer, secondPlayer);

            if (WinnerFound())
                return;

            RoundPostBattle(firstPlayer, secondPlayer);
        }

        private void AddRoundMana(Player player, int round)
        {
            const int Amount = 1;
            player.ManaChange(Amount);
            Observer.ShowPlayerManaChange(player, Amount);
        }

        private void PlayerTurn(Player player)
        {
            player.Turn++;

            Observer.BeforePlayerTurn(player);
            SendBattleFlowEvent(player, BattleFlowEvent.BeforeMyTurn);

            player.TakeTurn();

            SendBattleFlowEvent(player, BattleFlowEvent.AfterMyTurn);
        }

        public void CheckBattleEnded()
        {
            if (WinnerFound())
            {
                Observer.BattleEnded(Player1.IsDead ? Player2 : Player1, Player1.IsDead ? Player1 : Player2);
            }
        }

        public int HandleDeadCards()
        {
            int count = 0;
            foreach (var card in Board.AllCards())
            {
                if (card.IsDead)
                {
                    CardDied(card);
                    count++;
                }
            }
            return count;
        }

        public void CardDied(CardBasicMob card)
        {
            card.Owner.DefeatedCards.Add(card);
            Board.RemoveCard(card);
        }

        public void OutOfOrderCardTurn(Player src, int position)
        {
            BoardCardTurn(src, GetOpponent(src), position);
        }

        private void BoardCardTurn(Player src, Player dst, int position, int iteration = 0)
        {
            var cardSrc = Board.GetRow(src).Cards[position];
            var cardDst = Board.GetRow(dst).Cards[position];

            if (cardSrc == null || cardSrc.IsDead)
                return;

            if (cardDst == null || cardDst.IsDead)
            {
                if (dst.IsDead)
                    return;

                // Card vs. Player
                cardSrc.OnBattleFlowEvent(BattleFlowEvent.BeforeAttacking);

                int amount = dst.HpChange(-cardSrc.Dmg);
                if (dst.IsDead)
                {
                    cardSrc.BattleStatistics.KillingBlowOnEnemyHero = true;
                }

                cardSrc.OnBattleFlowEvent(BattleFlowEvent.AfterAttacking);

                Observer.BoardCardVsPlayer(src, dst, position, amount);
                cardSrc.OnDamageDone(amount);
            }
            else
            {
                // Card vs. card

                // Check sacrifice, protect, taunt, etc.
                SpellProperty redirectProp;
                var newCardDst = CheckDamageRedirect(cardSrc, cardDst, out redirectProp);
                if (newCardDst != cardDst)
                {
                    newCardDst.AddMainUiSpellOutput(redirectProp);
                    cardDst = newCardDst;
                }

                cardDst.OnBattleFlowEvent(BattleFlowEvent.BeforeAttacked);
                cardSrc.OnBattleFlowEvent(BattleFlowEvent.BeforeAttacking);

                int amount = cardDst.ChangeHp(-cardSrc.Dmg, cardDst.HpChangeType);

                if (redirectProp != null && redirectProp.Type == SpellPropertyType.Sacrifice)
                {
                    // Dst sacrificed itself
                    cardDst.ForceSetHp(0);
                }

                if (cardDst.IsDead)
                {
                    cardSrc.BattleStatistics.KillingBlowsOnEnemyCards++;
                }

                cardSrc.OnBattleFlowEvent(BattleFlowEvent.AfterAttacking);
                cardDst.OnBattleFlowEvent(BattleFlowEvent.AfterAttacked);

                Observer.BoardCardVsCard(cardSrc, cardDst, amount);
                cardSrc.OnDamageDone(amount, cardDst);
                if (cardDst.IsDead)
                {
                    CardDied(cardDst);
                }
            }

            if (cardSrc.HasSpellProperty(SpellPropertyType.DoubleAttack) && iteration == 0)
            {
                // Recursive call for double attack
                BoardCardTurn(src, dst, position, iteration + 1);
            }
        }

        private CardBasicMob CheckDamageRedirect(CardBasicMob src, CardBasicMob dst, out SpellProperty redirectProp)
        {
            redirectProp = null;
            var row = Board.GetRow(dst.Owner);

            List<CardBasicMob> candidates = new List<CardBasicMob>();
            foreach(var card in row.Cards)
            {
                if (card != null && card != dst)
                {
                    if (card.HasSpellProperty(SpellPropertyType.ProtectLeftAndRight))
                    {
                        if (Math.Abs(card.BoardPosition - dst.BoardPosition) == 1)
                        {
                            candidates.Add(card);
                        }
                    }

                    if (card.HasSpellProperty(SpellPropertyType.Sacrifice))
                    {
                        candidates.Add(card);
                    }

                    if (card.HasSpellProperty(SpellPropertyType.Taunt))
                    {
                        candidates.Add(card);
                    }
                }
            }

            if (candidates.Count() != 0)
            {
                // Prioritize candidates
                var taunter = FindClosest(dst, candidates.Where((c) => c.HasSpellProperty(SpellPropertyType.Taunt)));
                if (taunter != null)
                {
                    redirectProp = taunter.GetSpellProperty(SpellPropertyType.Taunt);
                    return taunter;
                }

                var protector = FindClosest(dst, candidates.Where((c) => c.HasSpellProperty(SpellPropertyType.ProtectLeftAndRight)));
                if (protector != null)
                {
                    redirectProp = protector.GetSpellProperty(SpellPropertyType.ProtectLeftAndRight);
                    return protector;
                }

                var sacrificer = FindClosest(dst, candidates.Where((c) => c.HasSpellProperty(SpellPropertyType.Sacrifice)));
                if (sacrificer != null)
                {
                    redirectProp = sacrificer.GetSpellProperty(SpellPropertyType.Sacrifice);
                    return sacrificer;
                }
            }

            return dst;
        }

        private CardBasicMob FindClosest(CardBasicMob card, IEnumerable<CardBasicMob> other)
        {
            CardBasicMob result = null;
            int minDiff = int.MaxValue;
            foreach(CardBasicMob o in other)
            {
                int diff = Math.Abs(card.BoardPosition - o.BoardPosition);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    result = o;
                }
            }
            return result;
        }

        void SendBattleFlowEvent(Player player, BattleFlowEvent flowEvent)
        {
            // Force into a list to be able to do changes while looping
            List<CardBasicMob> cardList = Board.GetRow(player).Cards.ToList();
            bool deadCardsFound = false;
            foreach (var card in cardList)
            {
                if (card != null)
                {
                    card.OnBattleFlowEvent(flowEvent);
                    if (card.IsDead)
                    {
                        deadCardsFound = true;
                        CardDied(card);
                    }
                }
            }

            if (deadCardsFound)
            {
                Observer.RemoveDeadCardsOnBoard();
            }
        }
    }
}
