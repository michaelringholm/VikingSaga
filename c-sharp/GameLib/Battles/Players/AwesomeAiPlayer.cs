using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using GameLib.Battles.Cards;
using GameLib.Battles.Interfaces;
using GameLib.Battles.Players.AI;

namespace GameLib.Battles
{
    class AwesomeAiPlayer : Player
    {
        private Battle _testBattle;
        private TestPlayer _testMe;
        private TestPlayer _testOpponent;
        private Player _opponent;

        private int _cardsPlayed;
        private AiPlay _bestPlay = null;

        public AwesomeAiPlayer()
        {
            AllPlays = new List<AiPlay>();
        }

        private void InitTestBattle()
        {
            TestPlayer player1;
            TestPlayer player2;
            AiHelper.CloneBattle(Battle, out player1, out player2, out _testBattle);

            _testMe = Battle.IsPlayer1(this) ? player1 : player2;
            _testOpponent = Battle.IsPlayer1(this) ? player2 : player1;
        }

        CardTargetGenerator _cardTargetGenerator = new CardTargetGenerator();

        private IEnumerable<CardBattle.CardTargetFlags> GeneratePotentialTargets(string cardOrdering, int maxPerCardOrdering)
        {
            _cardTargetGenerator.Reset(cardOrdering.Length);
            CardBattle.CardTargetFlags boardMobFlags = 0;

            int count = 0;
            while (true)
            {
                int i = 0;
                boardMobFlags = 0;
                while (i < cardOrdering.Length)
                {
                    char c = cardOrdering[i];
                    int handPosition = c - '0';
                    var card = Hand.Cards[handPosition];
                    bool isMob = card is CardBasicMob;
                    var potentialTargets = card.GetPotentialTargets();
                    var currentTarget = _cardTargetGenerator.Targets[i];

                    if (!potentialTargets.HasFlag(currentTarget) || (isMob && boardMobFlags.HasFlag(currentTarget)))
                    {
                        break;
                    }

                    if (CardBattle.RowOFlags.HasFlag(currentTarget))
                    {
                        // Check if desired row position is already taken
                        int rowPosition = AiHelper.IdxFromRowtarget(currentTarget);
                        if (Battle.Board.GetRow(this).Cards[rowPosition] != null)
                            break;
                    }

                    if (CardBattle.RowEFlags.HasFlag(currentTarget))
                    {
                        // Check if desired enemy target for instant card is not there
                        int rowPosition = AiHelper.IdxFromRowtarget(currentTarget);
                        if (Battle.Board.GetRow(_opponent).Cards[rowPosition] == null)
                            break;
                    }

                    if (isMob && currentTarget != CardBattle.CardTargetFlags.Null)
                    {
                        boardMobFlags |= currentTarget;
                    }

                    i++;
                }

                if (i == cardOrdering.Length)
                {
                    // All targets are valid
                    foreach (var target in _cardTargetGenerator.Targets)
                        yield return target;

                    if (count++ >= maxPerCardOrdering)
                        break;

                    if (!_cardTargetGenerator.Advance())
                        break;
                }
                else
                {
                    // Not all valid
                    if (!_cardTargetGenerator.AdvanceAt(i))
                        break;
                }
            }
        }

        private void ExecutePotentialTargets(string cardOrdering, List<CardBattle.CardTargetFlags> potentialTargets)
        {
            int i = 0;
            while (i < potentialTargets.Count)
            {
                InitTestBattle();
                int idxFirstTarget = i;

                for (int j = 0; j < cardOrdering.Length; ++j)
                {
                    char c = cardOrdering[j];
                    int handPosition = c - '0';
                    var card = _testMe.Hand.Cards[handPosition];
                    var target = potentialTargets[i++];
                    AiHelper.PlayCard(_testBattle, card, target);
                    _cardsPlayed++;
                }

                //   3) Execute combat
                _testBattle.DoTurns(_testBattle.FirstPlayer, _testBattle.SecondPlayer);

                ////   4) Execute post combat events
                _testBattle.RoundPostBattle(_testBattle.FirstPlayer, _testBattle.SecondPlayer);

                //   5) Evaluate score
                float score = CalcScore();

                var aiPlay = CreateAiPlay(
                    cardOrdering,
                    score,
                    potentialTargets.Skip(idxFirstTarget).Take(cardOrdering.Length),
                    _testMe.ScoreDebug,
                    _testOpponent.ScoreDebug);

                AllPlays.Add(aiPlay);

                if (score > _bestPlay.score)
                {
                    _bestPlay = CreateAiPlay(cardOrdering, score, potentialTargets.Skip(idxFirstTarget).Take(cardOrdering.Length), _testMe.ScoreDebug, _testOpponent.ScoreDebug);
                }
            }
        }

        private AiPlay CreateAiPlay(
            string cardOrdering,
            float score,
            IEnumerable<CardBattle.CardTargetFlags> targets,
            IEnumerable<string> myInfo,
            IEnumerable<string> enemyInfo)
        {
            var result = new AiPlay();
            result.scoreDebugInfoMe = new List<string>(myInfo);
            //result.scoreDebugInfoOther = new List<string>(enemyInfo);
            result.score = score;
            result.cardOrdering = cardOrdering;
            result.targets.Clear();
            result.targets.AddRange(targets);
            return result;
        }

        public List<AiPlay> AllPlays { get; private set; }

        private float CalcScore()
        {
            // Score differences in rows pre/post round
            float opponentRowScore = CalcRowDifference(Battle.Board.GetRow(_opponent), _testBattle.Board.GetRow(_testOpponent));
            float myRowScore = CalcRowDifference(Battle.Board.GetRow(this), _testBattle.Board.GetRow(_testMe));
            _testMe.UpdateScore(myRowScore, "Change in my Row");
            _testOpponent.UpdateScore(opponentRowScore, "Change in opponent row");

            // Score difference in enemy Hp pre/post round
            int opponentHeroHpChange = _opponent.Hp - _testBattle.GetOpponent(_testMe).Hp;
            _testMe.UpdateScore(opponentHeroHpChange, "Change in opponent Hero Hp");

            // Score difference in own spell properties pre/post round
            int spellPropValuePre = Battle.Board.GetRow(this).AllCards().Sum(c => c.SpellPropertyValue());
            int spellPropValuePost = _testBattle.Board.GetRow(_testMe).AllCards().Sum(c => c.SpellPropertyValue());
            _testMe.UpdateScore(spellPropValuePost - spellPropValuePre, "Row SpellProp change");

            return _testMe.Score;
        }

        private float CalcRowValue(BoardRow row)
        {
            float result = 0;
            foreach (var card in row.AllCards())
            {
                result += card.Hp;
                result += card.Dmg;
                result += card.CalcLocationBonus();
            }
            return result;
        }

        private float CalcRowDifference(BoardRow pre, BoardRow post)
        {
            float preScore = CalcRowValue(pre);
            float postScore = CalcRowValue(post);
            return postScore - preScore;
        }

        public override void TakeTurn()
        {
            _opponent = Battle.GetOpponent(this);

            AllPlays.Clear();

            string baseCardOrderString = CardCombinations.BuildCardOrderString(this.Hand);

            if (string.IsNullOrWhiteSpace(baseCardOrderString))
                return; // No cards in hand

            var allCardOrderings = CardCombinations.CreateAllCardOrderings(baseCardOrderString);

            Stopwatch sw = Stopwatch.StartNew();
            _bestPlay = new AiPlay();
            _cardsPlayed = 0;

            const int MaxPerCardOrdering = 1000; // Max number of solutiuons for this card ordering
            long nextInfoMs = 1000;

            foreach (string cardOrdering in allCardOrderings)
            {
                var potentialTargets = GeneratePotentialTargets(cardOrdering, MaxPerCardOrdering).ToList();
                ExecutePotentialTargets(cardOrdering, potentialTargets);
                if (sw.ElapsedMilliseconds > nextInfoMs)
                {
                    Observer.ShowPlayerInfo(this, "Hmmm...");
                    nextInfoMs = sw.ElapsedMilliseconds + 2000;
                }
            }

            long ms = sw.ElapsedMilliseconds;
            //Observer.AiDebug(AllPlays, (int)ms);

            // Play the selected targets
            for (int i = 0; i < _bestPlay.cardOrdering.Length; ++i)
            {
                char c = _bestPlay.cardOrdering[i];
                var target = _bestPlay.targets[i];
                AiHelper.PlayCommand(this, c, target);

                Observer.AiArtificialDelay();
            }
        }
    }
}
