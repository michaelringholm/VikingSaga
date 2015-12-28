using System;
using System.Linq;
using System.Collections.Generic;
using GameLib.Battles.Cards;
using GameLib.Battles.Interfaces;
using System.Diagnostics;

namespace GameLib.Battles.Players.AI
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
        public List<CardBattle.CardTargetFlags> targets = new List<CardBattle.CardTargetFlags>();
        public float score;
    }

    internal static class AiHelper
    {
        public static void PlayCommand(Player player, char cardPos, CardBattle.CardTargetFlags target)
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

        public static IEnumerable<CardBattle.CardTargetFlags> GetFlags(CardBattle.CardTargetFlags flags)
        {
            foreach (var flag in CardBattle.AllTargets)
            {
                if (flags.HasFlag(flag))
                    yield return flag;
            }
        }

        public static CardBattle.CardTargetFlags GetFirst(CardBattle.CardTargetFlags flags)
        {
            foreach(var flag in CardBattle.AllTargets)
            {
                if (flag != CardBattle.CardTargetFlags.Null && flags.HasFlag(flag))
                    return flag;
            }

            return CardBattle.CardTargetFlags.Null;
        }

        public static void PlayCard(Battle battle, CardBattle card, CardBattle.CardTargetFlags where)
        {
            if (!CardBattle.AllTargets.Contains(where))
                throw new ArgumentException(where + " is not allowed to contain multiple flags");

            if (where == CardBattle.CardTargetFlags.Null)
                return;

            var me = card.Owner;
            if (me.Mana < card.Power)
                return;

            // Drop on board
            int boardPosition = IdxFromRowtarget(where);
            if (boardPosition == -1)
                throw new InvalidOperationException(where + " is not a board position");

            var rowOwner = CardBattle.RowOFlags.HasFlag(where) ? me : battle.GetOpponent(me);
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
                Debug.Print(s);
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

        public static CardBattle.CardTargetFlags GetValidTargets(Battle battle, CardBattle card)
        {
            var o = card.Owner;
            var e = battle.GetOpponent(o);

            CardBattle.CardTargetFlags result = CardBattle.CardTargetFlags.Null;

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
                        result |= CardBattle.RowOList[i];
                    }
                }
                else
                {
                    if (card.CanTargetOwnCard)
                    {
                        // Drop on own card
                        result |= CardBattle.RowOList[i];
                    }
                }

                // Enemy row
                if (rowE.Cards[i] != null)
                {
                    if (card.CanTargetEnemyCard)
                    {
                        // Drop on enemy card
                        result |= CardBattle.RowEList[i];
                    }
                }
            }

            return result;
        }

        public static int IdxFromRowtarget(CardBattle.CardTargetFlags target)
        {
            switch(target)
            {
                case CardBattle.CardTargetFlags.BO0:
                case CardBattle.CardTargetFlags.BE0:
                    return 0;
                case CardBattle.CardTargetFlags.BO1:
                case CardBattle.CardTargetFlags.BE1:
                    return 1;
                case CardBattle.CardTargetFlags.BO2:
                case CardBattle.CardTargetFlags.BE2:
                    return 2;
                case CardBattle.CardTargetFlags.BO3:
                case CardBattle.CardTargetFlags.BE3:
                    return 3;
                case CardBattle.CardTargetFlags.BO4:
                case CardBattle.CardTargetFlags.BE4:
                    return 4;
                default:
                    return -1;
            }
        }
    }
}
