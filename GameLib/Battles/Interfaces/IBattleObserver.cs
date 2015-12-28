using System;
using System.Collections.Generic;
using VikingSaga.Code;
using GameLib.Battles.Cards;
using GameLib.Battles.Players.AI;

namespace GameLib.Battles
{
    public interface IBattleObserver
    {
        void BattleStart(Player firstPlayer);
        void BeforePlayerTurn(Player player);
        void ShowNotifications();
        void ShowPlayerInfo(Player player, string info);

        void ShowCardHpChange(CardBasicMob card, int amount);
        void ShowCardDmgChange(CardBasicMob card, int amount);

        void RemoveDeadCardsOnBoard();
        void CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi);

        void PartyCardPlaced(Player player, int position);
        void CardDrawn(Player player, int position);

        void DropCardOnCard(CardBattle src, CardBattle dst, object data, bool isAi);

        void BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount);

        void WaitForHumanEndTurn();
        void AiArtificialDelay();
        void BattleEnded(Player winner, Player loser);

        //void AiDebug(IEnumerable<AiPlay> plays, int ms);
    }
}
