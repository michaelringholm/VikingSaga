using System;
using System.Linq;
using System.Collections.Generic;

namespace GameLib.Battles.Players.AI
{
    public class CardCombinations
    {
        public static string BuildCardOrderString(Hand hand)
        {
            var result = string.Empty;
            foreach(var card in hand.AllCards())
            {
                result += card.HandPosition.ToString();
            }
            return result;
        }

        public static List<string> CreateAllCardOrderings(string cards)
        {
            var result = new List<string>();
            CreateAllCardOrderingsRecursiveImpl(string.Empty, cards, result);
            return result;
        }

        private static void CreateAllCardOrderingsRecursiveImpl(string settled, string rest, List<string> combos)
        {
            foreach (var card in rest)
            {
                var newSettled = settled + card;
                var newRest = RemoveChar(card, rest);

                if (newRest.Length == 0)
                {
                    combos.Add(newSettled);
                }
                else
                {
                    CreateAllCardOrderingsRecursiveImpl(newSettled, newRest, combos);
                }
            }
        }

        private static string RemoveChar(char c, string s)
        {
            return s.Remove(s.IndexOf(c), 1);
        }
    }
}