using System;
using VikingSaga.Code;

namespace VikingSagaWpfApp.Code.Battle
{
    public interface IBattleObserver
    {
        void BattleStart(Player firstPlayer);
        void RoundStart(int round);

        void BeforePlayerTurn(Player player);

        void CardDrawn(Player player, int cardIdx);
        void CardPlacedByAi(Player player, int handPosition, int boardPosition);
        void CardDragAndDropped(Card card, int handPosition, int boardPosition);

        void HandCardVsCard(Card card, BoardRow targetRow, int targetPosition);
        void HandCardVsPlayer(Card Card, Player targetPlayer);

        void BoardCardVsCard(Player src, Player dst, int position);
        void BoardCardVsPlayer(Player src, Player dst, int position);

        void CardHpChange(BoardRow row, int position, int amount);
        void PlayerHpChange(Player player, int amount);

        void PlayerDefeated(Player player);

        void WaitForHumanDecisions();
        void BattleEnded(Player winner);
    }
}
