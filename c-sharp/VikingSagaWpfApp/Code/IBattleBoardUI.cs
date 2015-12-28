using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VikingSagaWpfApp;
using VikingSagaWpfApp.Code.BattleNs;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSaga.Code
{
    public interface IBattleBoardUI : IUIControl
    {
        void WaitForHumanEndTurn();
        void CloseBattleBoard();
        void ShowBattleBoard();
        void ShowNotifications();
        void ShowPlayerInfo(Player player, string info);
        void SetStatusMessage(string msg, int ms = 1500);

        void UpdateEnemyHeroControl(Player player, Hero hero);
        void UpdateYourHeroControl(Player player, Hero hero);

        void ShowCardHpChange(CardBasicMob card, int amount);
        void ShowCardDmgChange(CardBasicMob card, int amount);
        void ShowPlayerHpChange(Player player, int amount);
        void ShowPlayerManaChange(Player player, int amount);

        // Either human drag or AI --->
        IEnumerable<int> ShowCardPlacedOnEmptyPlaceHolder(Player player, int handPosition, int boardPosition, bool isAi);
        IEnumerable<int> DropCardOnPlayer(Player dst, BattleCard card, object data, bool isAi);
        IEnumerable<int> DropCardOnCard(BattleCard src, BattleCard dst, object data, bool isAi);
        // <---

        IEnumerable<int> AddCardToHand(Player player, BattleCard card, int position);
        IEnumerable<int> CardVsCard(CardBasicMob src, CardBasicMob dst, int amount);
        IEnumerable<int> CardVsPlayer(Player src, Player dst, int position, int amount);
        IEnumerable<int> HandCardVsPlayer(CardInstant card, Player targetPlayer, int amount);
        IEnumerable<int> RemoveDeadCardsOnBoard();

        CardPlaceholder GetOverlappedPlaceholder(Rect rect, double minCoverage = 0.3);
        HeroCardControl GetOverlappedHero(Rect rect, double minCoverage = 0.3);

        void AiDebug(IEnumerable<AiPlay> plays, int ms);
    }
}
