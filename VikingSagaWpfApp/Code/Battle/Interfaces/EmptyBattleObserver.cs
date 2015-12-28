using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSagaWpfApp.Code.BattleNs.Interfaces
{
    public class EmptyBattleObserver : IBattleObserver
    {
        void IBattleObserver.BattleStart(Player firstPlayer) { }

        void IBattleObserver.BeforePlayerTurn(Player player) { }

        void IBattleObserver.ShowNotifications() { }

        void IBattleObserver.ShowPlayerInfo(Player player, string info) { }

        void IBattleObserver.ShowCardHpChange(CardBasicMob card, int amount) { }

        void IBattleObserver.CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi) { }

        void IBattleObserver.CardDrawn(Player player, int position) { }

        void IBattleObserver.DropCardOnPlayer(Player dst, BattleCard card, object data, bool isAi) { }

        void IBattleObserver.DropCardOnCard(BattleCard src, BattleCard dst, object data, bool isAi) { }

        void IBattleObserver.BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount) { }

        void IBattleObserver.BoardCardVsPlayer(Player src, Player dst, int position, int amount) { }

        void IBattleObserver.WaitForHumanDecisions() { }

        void IBattleObserver.AiArtificialDelay() { }

        void IBattleObserver.BattleEnded(Player winner, Player loser) { }

        void IBattleObserver.RemoveDeadCardsOnBoard() { }

        void IBattleObserver.ShowCardDmgChange(CardBasicMob card, int amount) { }

        void IBattleObserver.ShowPlayerManaChange(Player player, int amount) { }

        void IBattleObserver.AiDebug(IEnumerable<AiPlay> plays, int ms) { }
    }
}
