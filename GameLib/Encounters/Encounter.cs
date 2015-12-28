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
    public class Encounter
    {
        private GameEventManager _gameEventManager;

        public Encounter(GameEventManager gameEventManager)
        {
            _gameEventManager = gameEventManager;
            Party = new CardBattle[5];
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<CardBattle> Cards { get; set; }
        public CardBattle[] Party { get; set; }
        public Treasure Treasure { get; set; }
        public Card DisplayCard { get; set; }

        public bool HasCard(Type cardType)
        {
            if (Party.ToList().Exists(c => c != null && (c.GetType() == cardType)))
                return true;
            if (Cards.ToList().Exists(c => c.GetType() == cardType))
                return true;

            return false;
        }
    }
}
