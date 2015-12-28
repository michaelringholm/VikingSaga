using System;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp.Code.BattleNs.Players.AI
{
    public class CardTargetGenerator
    {
        public int Count { get; private set; }

        private static readonly int LastValue = 1 << (BattleCard.AllTargets.Count - 1);
        private static readonly CardTargetFlags InitialFlag = CardTargetFlags.Null;

        public void Reset(int count)
        {
            Count = count;
            Targets = new CardTargetFlags[Count];

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
                Targets[idx] = (CardTargetFlags)value;
                return AdvanceAt(idx - 1);
            }

            Targets[idx] = (CardTargetFlags)value;
            return true;
        }

        public CardTargetFlags[] Targets;
    }
}
