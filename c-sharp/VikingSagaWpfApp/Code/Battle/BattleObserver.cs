using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class BattleObserver : IBattleObserver
    {
        private IBattleBoardUI _battleBoardUI;

        public BattleObserver()
        {
            _battleBoardUI = GameController.Current.BattleBoardUI;
        }

        private Battle GetBattle()
        {
            return GameEngine.Current.CurrentBattle;
        }

        void UpdateUiHeroes()
        {
            var battle = GetBattle();
            _battleBoardUI.UpdateYourHeroControl(battle.Player1, GameController.Current.Profile.SelectedHero);
            _battleBoardUI.UpdateEnemyHeroControl(battle.Player2, GameEngine.Current.CurrentEncounter.Hero);
        }

        void IBattleObserver.BattleStart(Player firstPlayer)
        {
            UpdateUiHeroes();

            _battleBoardUI.SetStatusMessage(firstPlayer.Name + " begins", 2000);

            var battle = GetBattle();
            Status(string.Format("Player1 has {0} cards", battle.Player1.Deck.Cards.Count));
            Status(string.Format("Player2 has {0} cards", battle.Player2.Deck.Cards.Count));
        }

        private void Status(string s)
        {
            //Log.Line(s);
        }

        void IBattleObserver.CardDrawn(Player player, int position)
        {
            var battle = GetBattle();
            var card = player.Hand.Cards[position];
            SequentialActions.RunBlocking(_battleBoardUI.AddCardToHand(player, card, position));
        }

        void IBattleObserver.BeforePlayerTurn(Player player)
        {
            Status("BeforePlayerTurn : " + player.Name);
            var battle = GetBattle();
            if (battle.IsPlayer1(player))
            {
                _battleBoardUI.SetStatusMessage("Your turn, " + player.Name, 1500);
            }
        }

        void IBattleObserver.ShowPlayerInfo(Player player, string info)
        {
            _battleBoardUI.ShowPlayerInfo(player, info);
        }

        void IBattleObserver.ShowNotifications()
        {
            _battleBoardUI.ShowNotifications();
        }

        void IBattleObserver.RemoveDeadCardsOnBoard()
        {
            SequentialActions.RunBlocking(_battleBoardUI.RemoveDeadCardsOnBoard());
        }

        void IBattleObserver.CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi)
        {
            UpdateUiHeroes();
            SequentialActions.RunBlocking(_battleBoardUI.ShowCardPlacedOnEmptyPlaceHolder(card.Owner, handPosition, boardPosition, isAi));
        }

        void IBattleObserver.ShowCardHpChange(CardBasicMob card, int amount)
        {
            _battleBoardUI.ShowCardHpChange(card, amount);
        }

        void IBattleObserver.ShowCardDmgChange(CardBasicMob card, int amount)
        {
            _battleBoardUI.ShowCardDmgChange(card, amount);
        }

        void IBattleObserver.ShowPlayerManaChange(Player player, int amount)
        {
            _battleBoardUI.ShowPlayerManaChange(player, amount);
        }

        void IBattleObserver.DropCardOnPlayer(Player dst, BattleCard card, object data, bool isAi)
        {
            SequentialActions.RunBlocking(_battleBoardUI.DropCardOnPlayer(dst, card, data, isAi));
            GetBattle().CheckBattleEnded();
        }

        void IBattleObserver.DropCardOnCard(BattleCard src, BattleCard dst, object data, bool isAi)
        {
            SequentialActions.RunBlocking(_battleBoardUI.DropCardOnCard(src, dst, data, isAi));
            SequentialActions.RunBlocking(_battleBoardUI.RemoveDeadCardsOnBoard());
        }

        void IBattleObserver.BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount)
        {
            SequentialActions.RunBlocking(_battleBoardUI.CardVsCard(src, dst, amount));
            SequentialActions.RunBlocking(_battleBoardUI.RemoveDeadCardsOnBoard());
        }

        void IBattleObserver.BoardCardVsPlayer(Player src, Player dst, int position, int amount)
        {
            SequentialActions.RunBlocking(_battleBoardUI.CardVsPlayer(src, dst, position, amount));
        }

        void IBattleObserver.WaitForHumanDecisions()
        {
            _battleBoardUI.WaitForHumanEndTurn();
        }

        void IBattleObserver.AiArtificialDelay()
        {
            Thread.Sleep(300);
        }

        void IBattleObserver.BattleEnded(Player winner, Player loser)
        {
            Status("BattleEnded, winner : " + winner.Name);
            GameEngine.Current.OnBattleEnded(winner, loser);
        }

        void IBattleObserver.AiDebug(IEnumerable<AiPlay> plays, int ms)
        {
            _battleBoardUI.AiDebug(plays, ms);
        }
    }
}
