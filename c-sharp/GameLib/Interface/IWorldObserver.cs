using GameLib.Encounters;
using GameLib.Quests;
using GameLib.World.Maps;
using System;

namespace GameLib.Interface
{
    public interface IWorldObserver
    {
        void OnEnterMap(Map map);
        void OnQuestAdded(Quest quest);
        void OnQuestUpdated(Quest quest);
        void OnQuestCompleted(Quest quest);
        void OnQuestEncounterTriggered(Encounter encounter);
    }
}
