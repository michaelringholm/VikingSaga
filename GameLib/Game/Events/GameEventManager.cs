using GameLib.Quests;
using GameLib.Utility;
using System;
using System.Collections.Generic;

namespace GameLib.Game
{
    public class GameEventManager
    {
        public event EventHandler DirtyDateEvent;

        public event EventHandler<CardDefeatedEventArgs> CardDefeatedEvent;
        public event EventHandler<EncounterEventArgs> EncounterDefeatedEvent;

        public event EventHandler<QuestEventArgs> QuestEvent;
        public event EventHandler<QuestUpdatedEventArgs> QuestUpdatedEvent;
        public event EventHandler<QuestCompletedEventArgs> QuestCompletedEvent;
        public event EventHandler<NewQuestEventArgs> NewQuestEvent;
        public event EventHandler<EncounterEventArgs> QuestEncounterTriggeredEvent;

        public event EventHandler<SpecialLocationEventArgs> SpecialLocationEnteredEvent;
        public event EventHandler<SpecialLocationEventArgs> SpecialLocationLeftEvent;
        public event EventHandler<LocationEnteredEventArgs> LocationEnteredEvent;

        public void OnQuestEvent(Object sender, QuestEventArgs.QuestEventEnum questEventEnum)
        {
            KeyValueDebugInfo.SetItem("OnQuestEvent", questEventEnum);

            if (QuestEvent != null)
                QuestEvent(sender, new QuestEventArgs { QuestEvent = questEventEnum });
        }

        public void OnQuestUpdatedEvent(Object sender, QuestUpdatedEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnQuestUpdatedEvent", e.Quest.GetType().Name);
            if (QuestUpdatedEvent != null)
                QuestUpdatedEvent(sender, e);
        }

        public void OnQuestCompletedEvent(Object sender, QuestCompletedEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnQuestCompletedEvent", e.Quest.GetType().Name);
            if (QuestCompletedEvent != null)
                QuestCompletedEvent(sender, e);
        }

        public void OnNewQuestEvent(Object sender, NewQuestEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnNewQuestEvent", e.Quest.GetType().Name);
            if (NewQuestEvent != null)
                NewQuestEvent(sender, e);
        }

        public void OnQuestEncounterTriggeredEvent(Object sender, EncounterEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnQuestEncounterTriggeredEvent", e.Encounter.Title);
            if (QuestEncounterTriggeredEvent != null)
                QuestEncounterTriggeredEvent(sender, e);
        }

        public void OnDirtyDataEvent(Object sender)
        {
            KeyValueDebugInfo.SetItem("OnDirtyDataEvent", "-");
            if (DirtyDateEvent != null)
                DirtyDateEvent(sender, EventArgs.Empty);
        }

        public void OnCardDefeatedEvent(Object sender, CardDefeatedEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnCardDefeatedEvent", e.Card.GetType().Name);
            if (CardDefeatedEvent != null)
                CardDefeatedEvent(sender, e);
        }

        public void OnEncounterDefeatedEvent(Object sender, EncounterEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnEncounterDefeatedEvent", e.Encounter.Title);
            if (EncounterDefeatedEvent != null)
                EncounterDefeatedEvent(sender, e);
        }

        public void OnSpecialLocationEnteredEvent(Object sender, SpecialLocationEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnSpecialLocationEnteredEvent", e.Id);
            if (SpecialLocationEnteredEvent != null)
                SpecialLocationEnteredEvent(sender, e);
        }

        public void OnSpecialLocationLeftEvent(Object sender, SpecialLocationEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnSpecialLocationLeftEvent", e.Id);
            if (SpecialLocationLeftEvent != null)
                SpecialLocationLeftEvent(sender, e);
        }

        public void OnLocationEnteredEvent(Object sender, LocationEnteredEventArgs e)
        {
            KeyValueDebugInfo.SetItem("OnLocationEnteredEvent", e.LocationId);
            if (LocationEnteredEvent != null)
                LocationEnteredEvent(sender, e);
        }
    }
}
