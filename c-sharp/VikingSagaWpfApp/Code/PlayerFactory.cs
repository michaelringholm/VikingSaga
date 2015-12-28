using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players;

namespace VikingSagaWpfApp.Code
{
    internal static class PlayerFactory
    {
        public static IEnumerable<BattleCard> CreateBattleCards(IEnumerable<Card> cards)
        {
            var bcs = new List<BattleCard>();
            var eligibleCards = cards.Where(c => c.Condition == Card.CardConditionEnum.Perfect);
            foreach (Card c in eligibleCards)
            {
                BattleCard bc = VikingSagaWpfApp.Code.BattleNs.Cards.CardFactory.Create(c.BattleCardType);
                bc.ID = c.ID;
                bcs.Add(bc);
            }
            return bcs;
        }

        private static IEnumerable<BattleCard> CreateTestMobCards()
        {
            yield return new CardRat();
            yield return new CardRat();
            yield return new CardRat();
            yield return new CardRat();
            yield return new CardRat();
            yield return new CardSmallSpider();
            yield return new CardSmallSpider();
            yield return new CardSmallManaPotion();
            yield return new CardSmallHealingPotion();
        }

        private static IEnumerable<BattleCard> CreateTestMobCards2()
        {
            yield return new CardSmallRabbit();
            yield return new CardPeasant();
            yield return new CardSmallFox();
            yield return new CardPoison();
            yield return new CardDoubleAttack();
            yield return new CardSmallManaPotion();
            yield return new CardSmallManaPotion();
            yield return new CardRage();
            yield return new CardSacrifice();
            yield return new CardPoison();
            yield return new CardWarcry();
            yield return new CardAuraOfHealing();
            yield return new CardCataclysm();
            yield return new CardSmallFox();
            yield return new CardSmallFox();
            yield return new CardSmallManaPotion();
            yield return new CardSmallManaPotion();
        }

        private static IEnumerable<BattleCard> CreateTestInstantCards()
        {
            yield return new CardFireball();
            yield return new CardFireball();
            yield return new CardSmallHealingPotion();
            yield return new CardHeal1();
            yield return new CardWarcry();
            yield return new CardPoison();
            yield return new CardSmallManaPotion();
            yield return new CardSmallManaPotion();
            yield return new CardSmallManaPotion();
            yield return new CardHeal1();
            yield return new CardSmallHealingPotion();
        }

        private static IEnumerable<BattleCard> TestDeck()
        {
            yield return new CardPoison();
            yield return new CardRage();
            yield return new CardDoubleAttack();
            yield return new CardFireball();
            yield return new CardGhost();
            yield return new CardWarcry();
            yield return new CardAuraOfHealing();
            yield return new CardGhost();
            yield return new CardGhost();
            yield return new CardSmallManaPotion();
            yield return new CardGhost();
        }

        private static T CreatePlayerFromHero<T>(Hero hero) where T : Player, new()
        {
            var player = new T();
            player.Hp = hero.HP * 1;
            player.Mana = hero.Mana;
            player.Name = hero.Name;
            return player;
        }

        public static Player CreatePlayerFromProfile(VikingSagaUserProfile profile)
        {
            var player = CreatePlayerFromHero<HumanPlayer>(profile.SelectedHero);

            //var battleCards = TestDeck();
            var battleCards = CreateBattleCards(profile.Deck.Cards);
            player.Deck.SetCards(battleCards);

            return player;
        }

        public static Player CreatePlayerFromEncounter(Encounter encounter)
        {
            var player = CreatePlayerFromHero<AwesomeAiPlayer>(encounter.Hero);

            var battleCards = TestDeck();
            //var battleCards = CreateBattleCards(encounter.PlayableCards);
            player.Deck.SetCards(battleCards);

            return player;
        }
    }
}
