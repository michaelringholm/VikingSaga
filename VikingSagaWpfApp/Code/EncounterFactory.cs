using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    class EncounterFactory
    {
        public enum EncounterEnum { TrollBoss, TheMadBoar, BanditLeader, LargeRabbit };

        internal static Encounter GetRandomEncounter(Random random)
        {
            var encounterCards = new List<Card>();
            //Deck aiInitialDeck = new Deck();
            var deckCount = PickRandomDeckCount(random, 8);
            for (int i=0; i < deckCount; i++)
            {
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Rabbit1));
            }

            //encounterCards.Add(CardFactory.GetCard(CardFactory.MobTypeEnum.Rabbit1));
            var treasure = new Treasure { Gold = 1, XP = 600 };

            List<Hero> randomHeroes = new List<Hero>();
            randomHeroes.Add(new Warrior { Name = "A Peasant Leader", Level = 1, XP = 0, HP = 3, Mana = 4, CardImageURL = @"mobs\peasant.png" });
            randomHeroes.Add(new Warrior { Name = "An Alpha Rabbit", Level = 1, XP = 0, HP = 3, Mana = 3, CardImageURL = @"mobs\small-rabbit.png" });
            randomHeroes.Add(new Warrior { Name = "A Large Wild Boar", Level = 1, XP = 0, HP = 3, Mana = 3, CardImageURL = @"mobs\wild-boar.png" });
            randomHeroes.Add(new Warrior { Name = "A Large Fox", Level = 1, XP = 0, HP = 3, Mana = 3, CardImageURL = @"mobs\small-fox.png" });
            randomHeroes.Add(new Warrior { Name = "A Pixie Chieftain", Level = 1, XP = 0, HP = 3, Mana = 5, CardImageURL = @"mobs\pixie-female-nut.png" });

            var selectedHero = PickRandomHero(random, randomHeroes);

            var encounter = new AIEncounter { Hero = selectedHero, PlayableCards = encounterCards, Treasure = treasure, PreCombatText = "Making your way through the landscape, you have just stumbled upon " + selectedHero.Name + "." };

            return encounter;
        }

        private static Hero PickRandomHero(Random random, List<Hero> randomHeroes)
        {
            var count = randomHeroes.Count()-1;
            int index = Convert.ToInt16(Math.Round(((random.NextDouble()) * count)));

            return randomHeroes[index];
        }

        private static int PickRandomDeckCount(Random random, int max)
        {
            int count = Convert.ToInt16(Math.Round(((random.NextDouble()) * max)));

            if (count == 0)
                count = 1;

            return count;
        }

        internal static Encounter GetEncounter(EncounterEnum encounterEnum)
        {
            if (encounterEnum == EncounterEnum.TrollBoss)
            {
                var encounterCards = new List<Card>();
                var random = new Random();
                var deckCount = PickRandomDeckCount(random, 10);
                for (int i = 0; i < deckCount; i++)
                {
                    encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.WildBoar1));
                }

                //encounterCards.Add(CardFactory.GetCard(CardFactory.MobTypeEnum.Rabbit1));
                var treasure = new Treasure { Gold = 1, XP = 2 };
                var trollBoss = new Warrior { Name = "a mountain troll", Level = 2, XP = 0, HP = 12, Mana = 5, CardImageURL = @"mobs\cave-troll.png" };
                var encounter = new AIEncounter { Hero = trollBoss, PlayableCards = encounterCards, Treasure = treasure, PreCombatText = "Entering a dimly lit cave you feel yourself unable to flee, prepare to face " + trollBoss.Name + "!" };
                return encounter;
            }
            else if (encounterEnum == EncounterEnum.LargeRabbit)
            {
                var encounterCards = new List<Card>();
                var random = new Random();
                //var deckCount = PickRandomDeckCount(random, 10);
                var deckCount = 5;
                for (int i = 0; i < deckCount; i++)
                {
                    encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Rabbit1));
                }

                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.DiseasedRabbit1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.DiseasedRabbit1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.DiseasedRabbit1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.ElderRabbit1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.ElderRabbit1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Carrot1));
                encounterCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Carrot1));

                //encounterCards.Add(CardFactory.GetCard(CardFactory.MobTypeEnum.Rabbit1));
                var treasure = new Treasure { Gold = 1, XP = 2 };
                var largeRabbit = new Warrior { Name = "A Large Rabbit", Level = 1, XP = 0, HP = 10, Mana = 10, CardImageURL = @"mobs\small-rabbit.png" };
                var encounter = new AIEncounter { Hero = largeRabbit, PlayableCards = encounterCards, Treasure = treasure, PreCombatText = "Something jumps out of the nearby bushes, it is " + largeRabbit.Name + "!" };
                return encounter;
            }
            else
                throw new Exception("Undefined encounter");
        }
    }
}
