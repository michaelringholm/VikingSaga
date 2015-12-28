using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib.Battles.Cards;
using GameLib.Battles.Players.AI;

namespace GameLib.Battles.Interfaces
{
    public class EmptyBattleObserver : IBattleObserver
    {
        void IBattleObserver.BattleStart(Player firstPlayer) { }

        void IBattleObserver.BeforePlayerTurn(Player player) { }

        void IBattleObserver.ShowNotifications() { }

        void IBattleObserver.ShowPlayerInfo(Player player, string info) { }

        void IBattleObserver.ShowCardHpChange(CardBasicMob card, int amount) { }

        void IBattleObserver.CardPlacedOnBoard(CardBasicMob card, int handPosition, int boardPosition, bool isAi) { }

        void IBattleObserver.PartyCardPlaced(Player player, int position) { }

        void IBattleObserver.CardDrawn(Player player, int position) { }

        void IBattleObserver.DropCardOnCard(CardBattle src, CardBattle dst, object data, bool isAi) { }

        void IBattleObserver.BoardCardVsCard(CardBasicMob src, CardBasicMob dst, int amount) { }

        void IBattleObserver.WaitForHumanEndTurn() { }

        void IBattleObserver.AiArtificialDelay() { }

        void IBattleObserver.BattleEnded(Player winner, Player loser) { }

        void IBattleObserver.RemoveDeadCardsOnBoard() { }

        void IBattleObserver.ShowCardDmgChange(CardBasicMob card, int amount) { }

        //void IBattleObserver.AiDebug(IEnumerable<AiPlay> plays, int ms) { }
    }
}
