using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IGameEngineObserver
    {
        void BeforeFirstRound();
        void EnemyHeroTurnCompletedEvent();
        void EnemyHeroNewActiveCardPlayed(Card nextCard, Hero enemyHero, int cardIndex);
        void NewPlayableCardAdded(Card card, int cardIndex);
        void BattleLost(Encounter encounter);
        void BattleWon(Encounter encounter, MapLocation oldMapLocation);
        void EnemyHeroAttacked(Card activeCard, Hero enemyHero);
        void YourHeroAttacked(Encounter encounter, Card activeCard, Hero.ConditionEnum condition, Hero hero);
        void YourCardAttacked(int cardPosition, Card activeCard);
        void EnemyCardAttacked(int cardPosition, Card activeCard);
        void YourCardDefeated(int cardPosition);
        void EnemyCardDefeated(int cardPosition);
        void WaitForDecisions();
    }
}

