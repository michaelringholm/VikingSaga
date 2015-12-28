using GameLib.Battles;
using GameLib.Battles.Cards;
using GameLib.Encounters;
using GameLib.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Vik.Code.Controls;
using Vik.Code.Controls.Battle;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Game.Main.Battle
{
    public class EncounterController
    {
        public void RunEncounterAsync(Encounter encounter)
        {
            UiUtil.Post(() => { RunEncounter(encounter); });
        }

        public void RunEncounter(Encounter encounter)
        {
            PreBattle(encounter);

            var battleWindow = new BattleWindow(encounter);

            // HACK: Revive all before battle
            VikGame.World.PlayerProfile.SetNeedReviveFlag(VikGame.World.PlayerProfile.Data.PartyCards, false);

            var battle = CreateBattle(encounter, (IBattleObserver)battleWindow);
            battleWindow.SetBattle(battle);
            var battleResult = VikGame.ScreenManager.ShowContentModal(battleWindow);

            bool battleWon = battleResult == FakeWindow.Result.Yes;
            PostBattle(battleWon, encounter);
        }

        private GameLib.Battles.Battle CreateBattle(Encounter encounter, IBattleObserver battleObserver)
        {
            var humanPlayer = CreateHumanPlayer(encounter);
            var enemyPlayer = CreateEnemyplayer(encounter);

            var battle = new GameLib.Battles.Battle(humanPlayer, enemyPlayer, battleObserver);
            return battle;
        }

        private Player CreateHumanPlayer(Encounter encounter)
        {
            GameLib.Battles.Player player = new GameLib.Battles.HumanPlayer();
            player.Name = VikGame.World.PlayerProfile.Data.Name;
            var playerPartyCards = Card.CardsFromIds(VikGame.World.PlayerProfile.Data.PartyCards).Cast<CardBattle>().ToArray();
            player.Party = playerPartyCards;

            var playerDeckCards = Card.CardsFromIds(VikGame.World.PlayerProfile.Data.DeckCards).Cast<CardBattle>();
            player.Deck.SetCards(playerDeckCards);
            return player;
        }

        private Player CreateEnemyplayer(Encounter encounter)
        {
            GameLib.Battles.Player enemy = new GameLib.Battles.GenericAiPlayer();
            enemy.Name = encounter.Title;
            enemy.Deck.SetCards(encounter.Cards);
            enemy.Party = encounter.Party;
            return enemy;
        }

        private void PreBattle(Encounter encounter)
        {
            var preEncounterWin = new PreEncounterWindow(encounter);
            VikGame.ScreenManager.ShowContentModal(preEncounterWin, preEncounterWin.btnFight, true);
        }

        private void PostBattle(bool battleWon, Encounter encounter)
        {
            if (battleWon)
            {
                var encounterWinWindow = new EncounterWinWindow(encounter);
                VikGame.ScreenManager.ShowContentModal(encounterWinWindow, encounterWinWindow.btnOk, true);
                
                VikGame.World.GameEventManager.OnEncounterDefeatedEvent(this, new EncounterEventArgs { Encounter = encounter });

                VikGame.World.PlayerProfile.ReceiveTreasure(encounter.Treasure);
            }
            else
            {
                var encounterLoseWindow = new EncounterLoseWindow(encounter);
                VikGame.ScreenManager.ShowContentModal(encounterLoseWindow, encounterLoseWindow.btnOk, true);

                VikGame.World.PlayerProfile.SetNeedReviveFlag(VikGame.World.PlayerProfile.Data.PartyCards, true);
            }
            //OnPartyUpdated
        }
    }
}
