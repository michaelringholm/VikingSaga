using System;
using System.Linq;
using System.Collections.Generic;
using GameLib.Battles.Cards;
using GameLib.DataStore;
using GameLib.DataStore.DTOs;
using GameLib.Game;
using GameLib.Interface;
using GameLib.Quests;

namespace Vik.Code.Game.Main.Profiles
{
    public class Profile
    {
        public enum GlobalFlag
        {
            MidheimEntered = 1,
        }

        public ProfileDTO Data { get; private set; }

        private IGameDataStore _dataStore;
        private GameEventManager _gameEventManager;
        private IWorldObserver _worldObserver;

        public Profile(IGlobalData globalData)
        {
            _dataStore = globalData.DataStore;
            _gameEventManager = globalData.GameEventManager;
            _worldObserver = globalData.WorldObserver;

            _gameEventManager.DirtyDateEvent += GameEventManager_DirtyDateEvent;
            _gameEventManager.QuestUpdatedEvent += GameEventManager_QuestUpdatedEvent;
            _gameEventManager.QuestCompletedEvent += GameEventManager_QuestCompletedEvent;
            _gameEventManager.QuestEncounterTriggeredEvent += GameEventManager_QuestEncounterTriggeredEvent;
        }

        void GameEventManager_QuestEncounterTriggeredEvent(object sender, EncounterEventArgs e)
        {
            _worldObserver.OnQuestEncounterTriggered(e.Encounter);
        }

        void GameEventManager_QuestUpdatedEvent(object sender, QuestUpdatedEventArgs e)
        {
            _worldObserver.OnQuestUpdated(e.Quest);
        }

        // Raised by a quest that was just completed
        void GameEventManager_QuestCompletedEvent(object sender, QuestCompletedEventArgs e)
        {
            var quest = e.Quest;
            if (!Data.ActiveQuests.Contains(quest))
                throw new InvalidOperationException(string.Format("Quest (type = {0}) not found.", quest.GetType().Name));

            if (quest.Status != Quest.QuestStatus.Completed)
                throw new InvalidOperationException("Quest not completed, check quest logic: " + quest.GetTitle());

            quest.Status = Quest.QuestStatus.Completed;
            Data.ActiveQuests.Remove(quest);
            Data.CompletedQuestTypes.Add(quest.GetType().Name);

            _gameEventManager.OnDirtyDataEvent(this);

            _worldObserver.OnQuestCompleted(quest);
        }

        void GameEventManager_DirtyDateEvent(object sender, System.EventArgs e)
        {
            SetDataDirtyFlag();
        }

        private void SetDataDirtyFlag()
        {
            Data.Store();
            _dataStore.ForceCommit();
        }

        internal void LoadGame()
        {
            Data = ProfileDTO.LoadOrCreate<ProfileDTO>(_dataStore, DataStoreKey.PlayerProfile);
            InitializeActiveQuestsAfterLoad();
        }

        public bool HasGlobalFlag(Profile.GlobalFlag flag)
        {
            return Data.GlobalFlagsAsIntegers.Contains((int)flag);
        }

        public void SetGlobalFlag(Profile.GlobalFlag flag)
        {
            if (!HasGlobalFlag(flag))
                Data.GlobalFlagsAsIntegers.Add((int)flag);
        }

        public void SetNeedReviveFlag(string cardId, bool needRevive)
        {
            SetNeedReviveFlag(new string[] { cardId }, needRevive);
        }

        private bool SetPartyNeedReviveFlag(string cardId, bool needRevive)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (Data.PartyCards[i] == cardId)
                {
                    var card = Card.CardFromId(cardId);
                    if (card != null)
                    {
                        card.NeedRevive = needRevive;
                        Data.PartyCards[i] = Card.IdFromCard(card);
                        return true;
                    }
                }
            }
            return false;
        }

        public void ReviveAll()
        {
            SetNeedReviveFlag(Data.PartyCards, false);
            SetNeedReviveFlag(Data.RemainingCards, false);
        }

        public void SetNeedReviveFlag(IEnumerable<string> cardIds, bool needRevive)
        {
            foreach(var cardId in cardIds)
            {
                if (string.IsNullOrEmpty(cardId))
                    continue;

                if (!SetPartyNeedReviveFlag(cardId, needRevive))
                {
                    int idx = Data.RemainingCards.IndexOf(cardId);
                    if (idx >= 0)
                    {
                        var card = Card.CardFromId(cardId);
                        card.NeedRevive = needRevive;
                        Data.RemainingCards[idx] = Card.IdFromCard(card);
                    }
                    else
                    {
                        // Card id not found in party or remaining cards
                        throw new ArgumentException(string.Format("Card id {0} not found in party or remaining cards", cardId));
                    }
                }
            }
        }

        private void InitializeActiveQuestsAfterLoad()
        {
            foreach(var quest in Data.ActiveQuests)
            {
                quest.Initialize(_gameEventManager, Quest.QuestStatus.InProgress);
            }
        }

        internal void AddActiveQuest(Quest quest)
        {
            quest.Status = Quest.QuestStatus.InProgress;

            Data.ActiveQuests.Add(quest);

            _gameEventManager.OnDirtyDataEvent(this);
            _worldObserver.OnQuestAdded(quest);
        }

        public void ReceiveTreasure(Treasure reward)
        {
            Data.Gold += reward.Gold;
            Data.RemainingCards.AddRange(reward.Cards.Select(c => Card.IdFromCard(c)));
        }

        public static string[] StartingPartyCards()
        {
            string[] party = new string[5];
            party[0] = string.Empty;
            party[1] = CardBattle.IdFromType<CardDwarf>();
            party[2] = CardBattle.IdFromType<CardWolfPet>();
            party[3] = string.Empty;
            party[4] = string.Empty;
            return party;
        }

        public static IEnumerable<string> StartingDeckCards()
        {
            yield return CardBattle.IdFromType<CardHeal1>();
            yield return CardBattle.IdFromType<CardHeal1>();
            yield return CardBattle.IdFromType<CardStrength>();
            yield return CardBattle.IdFromType<CardHeal1>();
            yield return CardBattle.IdFromType<CardHeal1>();
        }

        public static IEnumerable<string> StartingRemainingCards()
        {
            yield return CardBattle.IdFromType<CardHeal1>();
            yield return CardBattle.IdFromType<CardStrength>();
            yield return CardBattle.IdFromType<CardHeal1>();
            yield return CardBattle.IdFromType<CardWarrior>();
            yield return CardBattle.IdFromType<CardPriest>();
            yield return CardBattle.IdFromType<CardDruid>();
            yield return CardBattle.IdFromType<CardMage>();
            yield return CardBattle.IdFromType<CardHunter>();
            yield return CardBattle.IdFromType<CardMinion>();
        }

        public void SellCard(Card card)
        {
            var exists = Data.RemainingCards.Exists(c => c == card.GetId());
            Data.RemainingCards.Remove(card.GetId());
            Data.Gold += card.Price;
            _gameEventManager.OnDirtyDataEvent(card);
        }
    }
}
