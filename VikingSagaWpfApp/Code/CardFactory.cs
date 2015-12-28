using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSaga.Code
{
    class CardFactory
    {
        internal static Deck CreateRandomDeck()
        {
            List<Card> starterCards = new List<Card>();
            int starterDeckSize = 10;
            Random random = new Random();
            var size = Enum.GetNames(typeof(MobTypeEnum)).Length;

            for (int i = 0; i < starterDeckSize; i++)
            {
                MobTypeEnum randomMobType = GetRandomMobType(random, size);
                starterCards.Add(CardFactory.CreateCard(randomMobType));
            }

            return new Deck { AllCards = starterCards, MaxDeckSize = starterDeckSize, Cards = starterCards };
        }

        internal static Deck CreateCampaignStarterDeck()
        {
            List<Card> starterCards = new List<Card>();
            int starterDeckSize = 10;
            Random random = new Random();
            var size = Enum.GetNames(typeof(MobTypeEnum)).Length;

            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Wolf1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Peasant1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Peasant1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Peasant1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Peasant1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.HealingPotion1));
            starterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.WarCry1));
            
            return new Deck { AllCards = starterCards, MaxDeckSize = starterDeckSize, Cards = starterCards };
        }

        private static MobTypeEnum GetRandomMobType(Random random, int numberOfMobs)
        {
            var index = random.Next(numberOfMobs);
            var name = Enum.GetNames(typeof(MobTypeEnum))[index];
            return (MobTypeEnum)Enum.Parse(typeof(MobTypeEnum), name);
        }

        public enum MobTypeEnum { Peasant1, Rabbit1, WildBoar1, Spider1, Pixie1, SkeletonWarrior1, Bear1, Fox1, ManaPotion1, HealingPotion1, Bandit1, Dwarf1, Ghost1, Leech1, Rat1, Raven1, Shadow1, Troll1, Worm1, Wolf1, WarCry1, DiseasedRabbit1, ElderRabbit1, Carrot1 };
        public static Card CreateCard(MobTypeEnum mobTypeEnum)
        {    
            Card card = null;
            if(mobTypeEnum == MobTypeEnum.Peasant1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardPeasant) };
            if(mobTypeEnum == MobTypeEnum.Rabbit1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallRabbit) };
            if (mobTypeEnum == MobTypeEnum.WildBoar1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardWildBoar) };
            if (mobTypeEnum == MobTypeEnum.Spider1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallSpider) };
            if (mobTypeEnum == MobTypeEnum.Pixie1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardPixie) };
            if (mobTypeEnum == MobTypeEnum.SkeletonWarrior1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSkeletonWarrior) };
            if (mobTypeEnum == MobTypeEnum.Bear1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallBear) };
            if (mobTypeEnum == MobTypeEnum.Fox1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallFox) };
            if (mobTypeEnum == MobTypeEnum.ManaPotion1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallManaPotion) };
            if (mobTypeEnum == MobTypeEnum.HealingPotion1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallHealingPotion) };
            if (mobTypeEnum == MobTypeEnum.Bandit1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardBandit) };
            if (mobTypeEnum == MobTypeEnum.Dwarf1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardDwarf) };
            if (mobTypeEnum == MobTypeEnum.Ghost1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardGhost) };
            if (mobTypeEnum == MobTypeEnum.Leech1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardLeech) };
            if (mobTypeEnum == MobTypeEnum.Rat1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardRat) };
            if (mobTypeEnum == MobTypeEnum.Raven1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardRaven) };
            if (mobTypeEnum == MobTypeEnum.Shadow1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardShadow) };
            if (mobTypeEnum == MobTypeEnum.Troll1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardTroll) };
            if (mobTypeEnum == MobTypeEnum.Worm1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardWorm) };
            if (mobTypeEnum == MobTypeEnum.Wolf1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardWolfPet) };
            if (mobTypeEnum == MobTypeEnum.WarCry1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardWarcry) };
            if (mobTypeEnum == MobTypeEnum.Carrot1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardSmallManaPotion) };
            if (mobTypeEnum == MobTypeEnum.ElderRabbit1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardElderRabbit) };
            if (mobTypeEnum == MobTypeEnum.DiseasedRabbit1)
                card = new Card { ID = Guid.NewGuid(), BattleCardType = typeof(CardDiseasedRabbit) };

            if (card == null)
                throw new Exception("Card enum type not implemented!");
            return card;
        }

        
    }
}