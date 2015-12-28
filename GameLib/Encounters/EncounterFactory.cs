using GameLib.Battles.Cards;
using GameLib.Game;
using GameLib.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Encounters
{
    public class EncounterFactory
    {
        public enum EncounterEnum { WolfPack1, WolfPack2, Narkrall }

        public static Encounter Create(EncounterEnum encounterType, GameEventManager gameEventManager)
        {
            var encounter = new Encounter(gameEventManager);

            encounter.Title = encounterType.ToString();
            encounter.Description = string.Format("Description of <B><C ORANGE>{0}<C DEFAULT></B>!!!", encounter.Title);
            encounter.Treasure = new Treasure();
            encounter.DisplayCard = new CardMinion{ Name = "<not set>", Description = "<not set>" };

            var cards = new List<CardBattle>();

            if(encounterType == EncounterEnum.WolfPack1)
            {
                encounter.DisplayCard = new CardWolfPet { Name = "Wolf Pack", Description = "A pack of wolves glares at you hungrily!" };
                for (int i = 0; i < 5; ++i)
                {
                    encounter.Party[i] = new CardWolfPet();
                }
                encounter.Cards = cards;
            }
            if (encounterType == EncounterEnum.Narkrall)
            {
                encounter.DisplayCard = new CardNarkrall();
                encounter.Title = encounterType.ToString();
                encounter.Description = string.Format("You face Narkrall Rakeclaw!", encounter.Title);
                encounter.Party[1] = new CardWildBoar();
                encounter.Party[2] = new CardNarkrall();
                encounter.Party[3] = new CardWildBoar();
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());
                cards.Add(new CardHeal1());

                var questRewardedCards = new List<Card>();
                questRewardedCards.Add(new CardWildBoar());
                questRewardedCards.Add(new CardWarcry());
                encounter.Treasure = new Treasure { Gold = 20, Cards = questRewardedCards };

                encounter.Cards = cards;
            }

            return encounter;
        }
    }
}
