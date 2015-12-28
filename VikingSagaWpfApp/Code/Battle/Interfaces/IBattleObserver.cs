using System;
using System.Collections.Generic;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public interface IBattleObserver
    {
        void BattleStart(Player firstPlayer);
        void BeforePlayerTurn(Player player);
        void ShowNotifications();
        void ShowPlayerInfo(Player player, string info);

        void ShowCardHpChange(CardBasicMob card, int amount);
        void ShowCardDmgChange(CardBasicMob card, int amount);
        void ShowPlayerManaChange(Player player, int amount);

        void RemoveDeadCardsOnBoard();
        void CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi);

        void CardDrawn(Player player, int position);

        void DropCardOnPlayer(Player dst, BattleCard card, object data, bool isAi);
        void DropCardOnCard(BattleCard src, BattleCard dst, object data, bool isAi);

        void BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount);
        void BoardCardVsPlayer(Player src, Player dst, int position, int amount);

        void WaitForHumanDecisions();
        void AiArtificialDelay();
        void BattleEnded(Player winner, Player loser);

        void AiDebug(IEnumerable<AiPlay> plays, int ms);
    }
}
