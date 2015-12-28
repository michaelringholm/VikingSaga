using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingSagaWpfApp.Code;
using VikingSagaWpfApp.Code.Battle;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSaga.Code
{
    public class GameEngine
    {
        public static void StartGameEngine(VikingSagaUserProfile profile)
        {
            if (Current == null)
            {
                var gameEngine = new GameEngine(profile);
                Current = gameEngine;
            }
        }

        private GameEngine(VikingSagaUserProfile profile) : this()
        {
            _profile = profile;
            ResetHero();
        }

        private GameEngine()
        {
            BattleEndedEvent += GameEngine_BattleEndedEvent;
        }

        public static GameEngine Current { get; private set; }

        #region Events
        public delegate void BattleEnded(Player winner, Player loser);
        public event BattleEnded BattleEndedEvent;
        public void OnBattleEnded(Player winner, Player loser)
        {
            if (BattleEndedEvent != null)
                BattleEndedEvent(winner, loser);
        }

        public delegate void BattleWonEventHandler(Encounter encounter, MapLocation oldMapLocation, MapLocation newMapLocation);
        public event BattleWonEventHandler BattleWonEvent;
        public void OnBattleWon(Encounter encounter, MapLocation oldMapLocation, MapLocation newMapLocation)
        {
            if (BattleWonEvent != null)
                BattleWonEvent(encounter, oldMapLocation, newMapLocation);
        }

        public delegate void BattleLostEventHandler(Encounter encounter);
        public event BattleLostEventHandler BattleLostEvent;
        public void OnBattleLost(Encounter encounter)
        {
            if (BattleLostEvent != null)
                BattleLostEvent(encounter);
        }

        public delegate void LevelGainedEventHandler();
        public event LevelGainedEventHandler LevelGainedEvent;
        public void OnLevelGained()
        {
            if (LevelGainedEvent != null)
                LevelGainedEvent();
        }        
        #endregion

        void GameEngine_BattleEndedEvent(Player winner, Player loser)
        {
            if (winner.Name == _profile.SelectedHero.Name)
            {
                BattleWon();
                UpdateCards(winner);
            }
            else
            {
                BattleLost();
                UpdateCards(loser);
            }
            
            ResetHero();
            _profile.Save();
        }

        private void ResetHero()
        {
            _profile.SelectedHero.RemainingHP = _profile.SelectedHero.HP;
            _profile.SelectedHero.RemainingMana = _profile.SelectedHero.Mana;
        }

        private void UpdateCards(Player humanPlayer)
        {
            foreach(BattleCard battleCard in humanPlayer.DefeatedCards)
            {
                _profile.Deck.AllCards.Single(c => c.ID == battleCard.ID).Condition = Card.CardConditionEnum.Defeated;
                _profile.Deck.Cards.Single(c => c.ID == battleCard.ID).Condition = Card.CardConditionEnum.Defeated;
                _profile.Deck.AllCards.Single(c => c.ID == battleCard.ID).BattlesLost++;
                _profile.Deck.Cards.Single(c => c.ID == battleCard.ID).BattlesLost++;
            }
        }

        public bool BattleInProgress { get; set; }

        internal void BattleLost()
        {
            _profile.SelectedHero.Defeat(CurrentEncounter.Treasure, _profile);
            BattleInProgress = false;
            OnBattleLost(CurrentEncounter);
            CurrentEncounter = null;
        }

        internal void BattleWon()
        {
            var oldMapLocation = Map.GetMapLocation(_profile.SelectedHero.Map, _profile.SelectedHero.Map.HeroCoordinates.X, _profile.SelectedHero.Map.HeroCoordinates.Y);

            if(PendingLocation != null)
                ChangeHeroMapPosition(_profile.SelectedHero, PendingLocation);
                        
            BattleInProgress = false;
            OnBattleWon(CurrentEncounter, oldMapLocation, PendingLocation);
            _profile.SelectedHero.Victory(CurrentEncounter.Treasure, _profile);
            CurrentEncounter = null;
            PendingLocation = null;
        }

        private Battle _battle;
        public Battle CurrentBattle { get { return _battle; } }

        internal void StartNewBattle(VikingSagaUserProfile profile, IBattleObserver battleObserver)
        {
            if (BattleInProgress)
                throw new Exception("Previous battle was never completed");
            else
            {
                CurrentEncounter = PendingEncounter;
                PendingEncounter = null;

                BattleInProgress = true;
                var player1 = PlayerFactory.CreatePlayerFromProfile(profile);
                var player2 = PlayerFactory.CreatePlayerFromEncounter(CurrentEncounter);

                _battle = new Battle(player1, player2, battleObserver);
                _battle.StartAsync();
            }
        }

        internal void Surrender()
        {
            BattleInProgress = false;
        }

        internal void CreateHero(VikingSagaUserProfile profile, string heroName, string selectedHeroClass)
        {
            Hero hero = null;
            if (selectedHeroClass.ToLower() == "warrior")
                hero = new Warrior { Name = heroName, Map = MapFactory.CreateMap(MapFactory.MapEnum.DefaultWorld) };
            else
                throw new Exception("Unknown hero class [" + selectedHeroClass + "]");
        }

        internal bool IsNewPositionConnected(Hero hero, MapLocation newMapLocation)
        {
            //hero.Map.HeroLocation = new MapLocation();
            //var heroMapLocation = Map.GetMapLocation(hero.Map, hero.Map.HeroCoordinates.X, hero.Map.HeroCoordinates.Y);
            //return heroMapLocation.ConnectedMapLocations.Contains(newMapLocation.Coordinates);
            var heroX = hero.Map.HeroCoordinates.X;
            var heroY = hero.Map.HeroCoordinates.Y;
            //var test = hero.Map.Locations.Where(l => l.ConnectedMapLocations.Count > 0).ToList<MapLocation>().Where(cm => cm.X == heroX && cm.Y == heroY).ToList());
            List<MapCoordinates> connectedCoordinates = new List<MapCoordinates>();
            foreach(var location in hero.Map.Locations)
            {
                var coordinates = location.ConnectedMapLocations.Where(cm => cm.X == heroX && cm.Y == heroY).ToList();
                if(coordinates.Count > 0)
                    connectedCoordinates.Add(location.Coordinates);
            }

            return connectedCoordinates.Count(cc => cc.X == newMapLocation.Coordinates.X && cc.Y == newMapLocation.Coordinates.Y) > 0;

            //return Map.CompareLocations(hero.Map.HeroCoordinates, newMapLocation.Coordinates);
        }

        internal void ChangeHeroMapPosition(Hero hero, MapLocation newMapLocation)
        {
            hero.Map.HeroCoordinates.X = newMapLocation.Coordinates.X;
            hero.Map.HeroCoordinates.Y = newMapLocation.Coordinates.Y;
        }

        public Encounter PendingEncounter { get; set; }
        public Encounter CurrentEncounter { get; set; }
        public MapLocation PendingLocation { get; set; }
        private VikingSagaUserProfile _profile { get; set; }

        internal void ReviveCard(Card cardToRevive)
        {
            foreach(Card card in _profile.Deck.AllCards)
            {
                card.Condition = Card.CardConditionEnum.Perfect;
            }

            foreach (Card card in _profile.Deck.Cards)
            {
                card.Condition = Card.CardConditionEnum.Perfect;
            }
        }

        internal void BuyCard(Card card)
        {
            _profile.Deck.AllCards.Add(CardFactory.CreateCard(CardFactory.MobTypeEnum.Rabbit1));
        }
    }
}
