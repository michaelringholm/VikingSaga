using GameLib.Battles.Cards;
using GameLib.Encounters;
using GameLib.Game;
using GameLib.World.Maps.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Quests
{
    public class QuestTheDemonChain : Quest
    {
        public override string GetTitle()
        {
            return "The Demon Chain";
        }

        public override string GetDescription()
        {
            return "Narkrall revealed to me the secret of his influence over the drakes in this area. I thought the Demon Chain was a myth, but he's had it all along." +
            "And now the fool is about to lose it!" +
            "His life is meaningless to me but the Dragonmaw can't afford to lose that artifact. Follow him north, up to The Bandit Kings Lair. The lair will be in chaos, so you may be able to land undetected. Return to me with the Demon Chain, even if you have to remove it from Narkrall's corpse." +
            "Find that artifact!";
        }

        private Card _questGiver = new CardDungeonMaster();
        public override Card GetQuestGiverCard()
        {
            return _questGiver;
        }

        public override string GetQuestCompletedDescription()
        {
            return @"Thank you very much adventurers you have done this village a great favor. Please accept this humble reward.";
        }

        public override Treasure GetQuestReward()
        {
            List<Card> rewardedCards = new List<Card>();
            rewardedCards.Add(new CardLeech());

            return new Treasure { Gold = 5, Cards = rewardedCards };
        }

        protected void HandleEncounterDefeated(EncounterEventArgs e)
        {
            if (Status == QuestStatus.InProgress && e.Encounter.HasCard(typeof(CardNarkrall)) && GetQuestProgress("NakrallKilled") == 0)
            {
                SetQuestProgress("NakrallKilled", 1);
                Status = QuestStatus.ReadyToTurnIn;
                QuestUpdated();
            }
        }

        private void HandleSpecialLocationEntered(SpecialLocationEventArgs e)
        {
            if (e.Id == World.WorldData.SpecialLocationEnum.Midheim)
            {
                if (Status == QuestStatus.ReadyToTurnIn && GetQuestProgress("NakrallKilled") == 1)
                    QuestCompleted();
                else if (Status == QuestStatus.Waiting)
                    NewQuest();
            }
        }

        private void HandleLocationEntered(LocationEnteredEventArgs e)
        {
            if (Status == QuestStatus.InProgress && e.Map is StartMap && e.LocationId == "OldFarm")
            {
                TriggerEncounter(EncounterFactory.EncounterEnum.Narkrall);
            }
        }

        public override void UnHookEvents()
        {
            _gameEventManager.EncounterDefeatedEvent -= _gameEventManager_EncounterDefeatedEvent;
            _gameEventManager.SpecialLocationEnteredEvent -= _gameEventManager_SpecialLocationEnteredEvent;
            _gameEventManager.LocationEnteredEvent -= _gameEventManager_LocationEnteredEvent;
        }

        protected override void HookupEvents()
        {
            _gameEventManager.EncounterDefeatedEvent += _gameEventManager_EncounterDefeatedEvent;
            _gameEventManager.SpecialLocationEnteredEvent += _gameEventManager_SpecialLocationEnteredEvent;
            _gameEventManager.LocationEnteredEvent += _gameEventManager_LocationEnteredEvent;
        }

        void _gameEventManager_LocationEnteredEvent(object sender, LocationEnteredEventArgs e)
        {
            HandleLocationEntered(e);
        }

        void _gameEventManager_SpecialLocationEnteredEvent(object sender, SpecialLocationEventArgs e)
        {
            HandleSpecialLocationEntered(e);
        }

        void _gameEventManager_EncounterDefeatedEvent(object sender, EncounterEventArgs e)
        {
            HandleEncounterDefeated(e);
        }
    }
}
