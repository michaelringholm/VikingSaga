using System;
using System.Collections.Generic;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    [Flags]
    public enum CardTargetFlags
    {
        Null = 1 << 0,
        PO = 1 << 1,
        PE = 1 << 2,
        BO0 = 1 << 3,
        BO1 = 1 << 4,
        BO2 = 1 << 5,
        BO3 = 1 << 6,
        BO4 = 1 << 7,
        BE0 = 1 << 8,
        BE1 = 1 << 9,
        BE2 = 1 << 10,
        BE3 = 1 << 11,
        BE4 = 1 << 12,
    };

    public enum HpChangeType { Physical, Magic, Fire, Ice, Disease, Poison };

    public class CardBattleStatistics
    {
        public int KillingBlowsOnEnemyCards { get; set; }
        public bool KillingBlowOnEnemyHero { get; set; }
        public bool WasInHand { get; set; }
        public bool WasOnBoard { get; set; }
    }

    public class BattleCard
    {
        public IBattleObserver Observer { get; set; }
        public List<string> UiOutput { get; private set; }
        public bool EnableUiOutput { get; set; }
        public Guid ID { get; set; }
        public CardBattleStatistics BattleStatistics { get; private set; }

        public bool CanTargetOwnBoard { get; protected set; }
        public bool CanTargetOwnCard { get; protected set; }
        public bool CanTargetOwnPlayer { get; protected set; }
        public bool CanTargetEnemyCard { get; protected set; }
        public bool CanTargetEnemyPlayer { get; protected set; }

        public Player Owner { get; set; }
        public int HandPosition { get; set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string ImageUrl { get; set; }
        public int Mana { get; protected set; }

        public bool IsClone { get; set; }

        public BattleCard()
        {
            BattleStatistics = new CardBattleStatistics();
            UiOutput = new List<string>();

            Name = "BattleCard";
            Description = string.Empty;
            ImageUrl = "unknown.png";
            HandPosition = -1;
            Mana = 1;
        }

        public virtual BattleCard Clone()
        {
            var clone = (BattleCard)this.MemberwiseClone();
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

        private static CardTargetFlags GetPotentialTargets(BattleCard card)
        {
            CardTargetFlags result = CardTargetFlags.Null;

            if (card.CanTargetOwnPlayer)
                result |= CardTargetFlags.PO;

            if (card.CanTargetEnemyPlayer)
                result |= CardTargetFlags.PE;

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
                    result &= ~CardTargetFlags.PE;
                }
                else
                {
                    result &= ~RowOFlags;
                    result &= ~CardTargetFlags.PO;
                }
            }

            return result;
        }

    }
}
