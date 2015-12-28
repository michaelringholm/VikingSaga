using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Battles.Cards
{
    public class CardBattleStatistics
    {
        public int KillingBlowsOnEnemyCards { get; set; }
        public bool KillingBlowOnEnemyHero { get; set; }
        public bool WasInHand { get; set; }
        public bool WasOnBoard { get; set; }
    }

    public class CardBattle : Card
    {
        [Flags]
        public enum CardTargetFlags
        {
            Null = 1 << 0,
            BO0 = 1 << 1,
            BO1 = 1 << 2,
            BO2 = 1 << 3,
            BO3 = 1 << 4,
            BO4 = 1 << 5,
            BE0 = 1 << 6,
            BE1 = 1 << 7,
            BE2 = 1 << 8,
            BE3 = 1 << 9,
            BE4 = 1 << 10,
        }; 

        public enum HpChangeTypeEnum { Physical, Magic, Fire, Ice, Disease, Poison };
        
        public IBattleObserver Observer { get; set; }
        public List<string> UiOutput { get; private set; }
        public bool EnableUiOutput { get; set; }
        public CardBattleStatistics BattleStatistics { get; private set; }

        public bool CanTargetOwnBoard { get; protected set; }
        public bool CanTargetOwnCard { get; protected set; }
        public bool CanTargetEnemyCard { get; protected set; }

        public Player Owner { get; set; }
        public int HandPosition { get; set; }

        public int Power { get; protected set; }

        public bool IsClone { get; set; }        

        public CardBattle()
        {
            BattleStatistics = new CardBattleStatistics();
            UiOutput = new List<string>();

            Name = "BattleCard";
            Description = string.Empty;
            ImageUrl = "unknown.png";
            HandPosition = -1;
            Power = 1;
        }

        public virtual CardBattle Clone()
        {
            var clone = (CardBattle)this.MemberwiseClone();
            clone.BattleStatistics = new CardBattleStatistics();
            clone.UiOutput = new List<string>();
            clone.IsClone = true;
            return clone;
        }

        public virtual void Init() { }

        public void AddUiOutput(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && UiOutput != null)
                UiOutput.Add(text);
        }

        public virtual void OnDamageDone(int amount) { }
        public virtual void OnDamageDone(int amount, CardBasicMob dst) { }
        public virtual void OnBattleFlowEvent(BattleFlowEvent flowEvent, object data = null) { }

        public static List<CardTargetFlags> AllTargets = new List<CardTargetFlags>((CardTargetFlags[])Enum.GetValues(typeof(CardTargetFlags)));
        public static List<CardTargetFlags> RowOList = new List<CardTargetFlags> { CardTargetFlags.BO0, CardTargetFlags.BO1, CardTargetFlags.BO2, CardTargetFlags.BO3, CardTargetFlags.BO4 };
        public static List<CardTargetFlags> RowEList = new List<CardTargetFlags> { CardTargetFlags.BE0, CardTargetFlags.BE1, CardTargetFlags.BE2, CardTargetFlags.BE3, CardTargetFlags.BE4 };
        public static readonly CardTargetFlags RowOFlags = CardTargetFlags.BO0 | CardTargetFlags.BO1 | CardTargetFlags.BO2 | CardTargetFlags.BO3 | CardTargetFlags.BO4;
        public static readonly CardTargetFlags RowEFlags = CardTargetFlags.BE0 | CardTargetFlags.BE1 | CardTargetFlags.BE2 | CardTargetFlags.BE3 | CardTargetFlags.BE4;

        public CardTargetFlags GetPotentialTargets()
        {
            return GetPotentialTargets(this);
        }

        private static CardTargetFlags GetPotentialTargets(CardBattle card)
        {
            CardTargetFlags result = CardTargetFlags.Null;

            if (card.CanTargetOwnBoard || card.CanTargetOwnCard)
                result |= RowOFlags;

            if (card.CanTargetEnemyCard)
                result |= RowEFlags;

            // Performance: For positive effects filter out enemy row/player, for negative effects filter out own row/player
            var instantCard = card as CardInstant;
            if (instantCard != null)
            {
                if (instantCard.Effect == SpellProperty.Result.Positive)
                {
                    result &= ~RowEFlags;
                }
                else
                {
                    result &= ~RowOFlags;
                }
            }

            return result;
        }

    }
}
