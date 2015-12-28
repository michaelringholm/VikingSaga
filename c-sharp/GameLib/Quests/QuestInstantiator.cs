using GameLib.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vik.Code.Game.Main.Profiles;

namespace GameLib.Quests
{
    internal class QuestInstantiator
    {
        private List<Quest> _instantiatedQuests = new List<Quest>();
        private GameEventManager _gameEventManager;
        private Profile _playerProfile;

        public QuestInstantiator(IEnumerable<string> completedQuestTypes, GameEventManager gameEventManager, Profile playerProfile)
        {
            _gameEventManager = gameEventManager;
            _playerProfile = playerProfile;

            var questTypes = FindDerivedTypes(Assembly.GetExecutingAssembly(), typeof(Quest));

            // Only instantiate quests that are not completed and are not in the active quests list
            var withoutCompleted = questTypes.Where(questType => !completedQuestTypes.Contains(questType.Name));
            foreach (var questType in withoutCompleted)
            {
                if (playerProfile.Data.ActiveQuests.Exists(ac => ac.GetType() == questType))
                    continue;

                var quest = (Quest)Activator.CreateInstance(questType);
                quest.Initialize(gameEventManager, Quest.QuestStatus.Waiting);

                _instantiatedQuests.Add(quest);
            }

            gameEventManager.NewQuestEvent += gameEventManager_NewQuestEvent;
        }

        // Raised by a quest that wants to be added to the players active quests
        void gameEventManager_NewQuestEvent(object sender, NewQuestEventArgs e)
        {
            var quest = e.Quest;
            if (!_instantiatedQuests.Contains(quest))
                throw new InvalidOperationException(string.Format("Quest (type = {0}) not found.", quest.GetType().Name));

            _instantiatedQuests.Remove(quest);
            _playerProfile.AddActiveQuest(quest);
        }

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType);
        }

        public void UnhookEvents()
        {
            _gameEventManager.NewQuestEvent -= gameEventManager_NewQuestEvent;

            foreach (var quest in _instantiatedQuests)
            {
                quest.UnHookEvents();
            }
        }
    }
}
