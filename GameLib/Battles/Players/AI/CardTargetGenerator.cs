using System;
using GameLib.Battles.Cards;

namespace GameLib.Battles.Players.AI
{
    public class CardTargetGenerator
    {
        public int Count { get; private set; }

        private static readonly int LastValue = 1 << (CardBattle.AllTargets.Count - 1);
        private static readonly CardBattle.CardTargetFlags InitialFlag = CardBattle.CardTargetFlags.Null;

        public void Reset(int count)
        {
            Count = count;
            Targets = new CardBattle.CardTargetFlags[Count];

            for (int i = 0; i < Count; ++i)
            {
                Targets[i] = InitialFlag;
            }
        }

        public bool Advance()
        {
            return AdvanceAt(Count - 1);
        }

        public bool AdvanceAt(int idx)
        {
            int value = (int)Targets[idx];
            value <<= 1;

            if (value > LastValue)
            {
                if (idx == 0)
                    return false;

                value = (int)InitialFlag;
                Targets[idx] = (CardBattle.CardTargetFlags)value;
                return AdvanceAt(idx - 1);
            }

            Targets[idx] = (CardBattle.CardTargetFlags)value;
            return true;
        }

        public CardBattle.CardTargetFlags[] Targets;
    }
}
