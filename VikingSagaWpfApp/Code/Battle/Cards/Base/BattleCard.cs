using System;
using System.Collections.Generic;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
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

        public virtual void Init() { }

        public void AddUiOutput(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                UiOutput.Add(text);
        }

        public virtual void OnDamageDone(int amount) { }
        public virtual void OnDamageDone(int amount, CardBasicMob dst) { }
        public virtual void OnBattleFlowEvent(BattleFlowEvent flowEvent, object data = null) { }
    }
}
