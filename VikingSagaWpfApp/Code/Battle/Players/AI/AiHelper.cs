using System;
using System.Linq;
using System.Collections.Generic;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Interfaces;

namespace VikingSagaWpfApp.Code.BattleNs.Players.AI
{
    public class AiPlay
    {
        public AiPlay()
        {
            score = int.MinValue;
            cardOrdering = string.Empty;
        }

        public string TargetString()
        {
            return string.Format("{0} -> {1} (Score: {2:0.0})", cardOrdering, string.Join(", ", targets), score);
        }

        public List<string> scoreDebugInfoMe = new List<string>();
        public List<string> scoreDebugInfoOther = new List<string>();
        public string cardOrdering;
        public List<CardTargetFlags> targets = new List<CardTargetFlags>();
        public float score;
    }

    internal static class AiHelper
    {
        public static void PlayCommand(Player player, char cardPos, CardTargetFlags target)
        {
            int handPosition = cardPos - '0';
            var card = player.Hand.Cards[handPosition];
            AiHelper.PlayCard(player.Battle, card, target);
        }

        public static void CloneBattle(Battle battle, out TestPlayer clonePlayer1, out TestPlayer clonePlayer2, out Battle cloneBattle)
        {
            var emptyObserver = new EmptyBattleObserver();

            clonePlayer1 = new TestPlayer();
            clonePlayer1.Observer = emptyObserver;
            clonePlayer1.CopyFrom(battle.Player1);

            clonePlayer2 = new TestPlayer();
            clonePlayer2.Observer = emptyObserver;
            clonePlayer2.CopyFrom(battle.Player2);

            cloneBattle = new Battle(clonePlayer1, clonePlayer2, emptyObserver);
            cloneBattle.CopyFrom(battle);
        }

        public static IEnumerable<CardTargetFlags> GetFlags(CardTargetFlags flags)
        {
            foreach (var flag in BattleCard.AllTargets)
            {
                if (flags.HasFlag(flag))
                    yield return flag;
            }
        }

        public static CardTargetFlags GetFirst(CardTargetFlags flags)
        {
            foreach(var flag in BattleCard.AllTargets)
            {
                if (flag != CardTargetFlags.Null && flags.HasFlag(flag))
                    return flag;
            }

            return CardTargetFlags.Null;
        }

        public static void PlayCard(Battle battle, BattleCard card, CardTargetFlags where)
        {
            if (!BattleCard.AllTargets.Contains(where))
                throw new ArgumentException(where + " is not allowed to contain multiple flags");

            if (where == CardTargetFlags.Null)
                return;

            var me = card.Owner;
            if (me.Mana < card.Mana)
                return;

            // Drop on player
            if (where == CardTargetFlags.PO || where == CardTargetFlags.PE)
            {
                var dst = where == CardTargetFlags.PO ? me : battle.GetOpponent(me);
                me.DropCardOnPlayer(dst, (CardInstant)card, true);
                return;
            }

            // Drop on board
            int boardPosition = IdxFromRowtarget(where);
            if (boardPosition == -1)
                throw new InvalidOperationException(where + " is not a board position");

            var rowOwner = BattleCard.RowOFlags.HasFlag(where) ? me : battle.GetOpponent(me);
            var dstCard = battle.Board.GetCardAt(rowOwner, boardPosition);
            if (dstCard != null)
            {
                // Drop instant card on card on board
                me.DropCardOtherCard((CardInstant)card, dstCard, true);
            }
            else
            {
                // An instant, fx. Fireball, may be dropped anywhere on board - if there is a target card. Since there is currently not, just return.
                if (!(card is CardBasicMob))
                    return;

                // Drop mob card on empty spot on board
                me.DropCardOnBoard((CardBasicMob)card, card.HandPosition, boardPosition, true);
            }
        }

        public static void DumpState(Battle battle)
        {
            foreach(string s in GetState(battle))
            {
                Log.Line(s);
            }
        }

        private static IEnumerable<string> GetState(Battle battle)
        {
            yield return string.Format("P1.Deck  : " + string.Join(", ", battle.Player1.Deck.Cards.Select(c => c.Name)));
            yield return string.Format("P2.Deck  : " + string.Join(", ", battle.Player2.Deck.Cards.Select(c => c.Name)));
            yield return string.Format("P1.Hand  : " + string.Join(", ", battle.Player1.Hand.AllCards().Select(c => c.Name)));
            yield return string.Format("P2.Hand  : " + string.Join(", ", battle.Player2.Hand.AllCards().Select(c => c.Name)));
            yield return string.Format("P1.Board : " + string.Join(", ", battle.Board.GetRow(battle.Player1).AllCards().Select(c => c.Name)));
            yield return string.Format("P2.Board : " + string.Join(", ", battle.Board.GetRow(battle.Player2).AllCards().Select(c => c.Name)));
        }

        public static CardTargetFlags GetValidTargets(Battle battle, BattleCard card)
        {
            var o = card.Owner;
            var e = battle.GetOpponent(o);

            CardTargetFlags result = CardTargetFlags.Null;

            if (card.CanTargetOwnPlayer)
                result |= CardTargetFlags.PO;

            if (card.CanTargetEnemyPlayer)
                result |= CardTargetFlags.PE;

            var rowO = battle.Board.GetRow(o);
            var rowE = battle.Board.GetRow(e);

            for (int i = 0; i < 5; ++i)
            {
                // Own row
                if (rowO.Cards[i] == null)
                {
                    if (card.CanTargetOwnBoard)
                    {
                        // Drop on empty board spot
                        result |= BattleCard.RowOList[i];
                    }
                }
                else
                {
                    if (card.CanTargetOwnCard)
                    {
                        // Drop on own card
                        result |= BattleCard.RowOList[i];
                    }
                }

                // Enemy row
                if (rowE.Cards[i] != null)
                {
                    if (card.CanTargetEnemyCard)
                    {
                        // Drop on enemy card
                        result |= BattleCard.RowEList[i];
                    }
                }
            }

            return result;
        }

        public static int IdxFromRowtarget(CardTargetFlags target)
        {
            switch(target)
            {
                case CardTargetFlags.BO0:
                case CardTargetFlags.BE0:
                    return 0;
                case CardTargetFlags.BO1:
                case CardTargetFlags.BE1:
                    return 1;
                case CardTargetFlags.BO2:
                case CardTargetFlags.BE2:
                    return 2;
                case CardTargetFlags.BO3:
                case CardTargetFlags.BE3:
                    return 3;
                case CardTargetFlags.BO4:
                case CardTargetFlags.BE4:
                    return 4;
                default:
                    return -1;
            }
        }
    }
}
