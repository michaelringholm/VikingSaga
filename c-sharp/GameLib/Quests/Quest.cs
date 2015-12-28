using GameLib.Battles.Cards;
using GameLib.Encounters;
using GameLib.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameLib.Quests
{
    // All quest types must be added here or serialization will throw an error. Sucks :-(
    [XmlInclude(typeof(QuestTheDemonChain))]

    public abstract class Quest
    {
        public enum QuestStatus { Waiting, InProgress, ReadyToTurnIn, Completed };

        public Quest() { }

        protected GameEventManager _gameEventManager;

        public QuestStatus Status { get; set; }

        public void Initialize(GameEventManager gameEventManager, QuestStatus status)
        {
            if (QuestProgressItems != null)
            {
                throw new InvalidOperationException("Initialize already called on this quest: " + GetTitle());
            }

            QuestProgressItems = new List<QuestProgressItem>();
            _gameEventManager = gameEventManager;
            Status = status;
            HookupEvents();
        }

        protected abstract void HookupEvents();
        public abstract void UnHookEvents();

        protected void QuestUpdated()
        {            
            _gameEventManager.OnQuestUpdatedEvent(this, new QuestUpdatedEventArgs { Quest = this });
            _gameEventManager.OnDirtyDataEvent(this);
        }

        protected void QuestCompleted()
        {
            this.Status = QuestStatus.Completed;
            _gameEventManager.OnQuestCompletedEvent(this, new QuestCompletedEventArgs { Quest = this });
            _gameEventManager.OnDirtyDataEvent(this);
        }

        protected void NewQuest()
        {            
            _gameEventManager.OnNewQuestEvent(this, new NewQuestEventArgs { Quest = this });
            _gameEventManager.OnDirtyDataEvent(this);
        }

        protected void TriggerEncounter(EncounterFactory.EncounterEnum encounterType)
        {
            var encounter = EncounterFactory.Create(encounterType, _gameEventManager);
            _gameEventManager.OnQuestEncounterTriggeredEvent(this, new EncounterEventArgs { Encounter = encounter });
        }

        private List<QuestProgressItem> QuestProgressItems { get; set; }

        protected void SetQuestProgress(String objective, int progress)
        {
            GetQuestProgressItem(objective).Progress = progress;
        }

        protected int GetQuestProgress(String objective)
        {
            return GetQuestProgressItem(objective).Progress;
        }

        private QuestProgressItem GetQuestProgressItem(String objective)
        {
            var questProgressItem = QuestProgressItems.SingleOrDefault(qpi => qpi.Objective == objective);
            if (questProgressItem == null)
            {
                questProgressItem = new QuestProgressItem { Objective = objective, Progress = 0 };
                QuestProgressItems.Add(questProgressItem);
            }
            
            return questProgressItem;
        }

        public abstract string GetTitle();
        public abstract string GetDescription();
        public abstract string GetQuestCompletedDescription();
        public abstract Card GetQuestGiverCard();

        public abstract Treasure GetQuestReward();
    }
}
