using System;
using VikingSagaWpfApp.Controls;
using VikingSagaWpfApp.Windows;
using VikingSagaWpfApp.Code;
using VikingSagaWpfApp.Code.Battle;
using VikingSagaWpfApp;

namespace VikingSaga.Code
{
    public class GameController
    {
        public static readonly double GameSpeed = 2.0;

        public static readonly BattleObserver BattleController = new BattleObserver();
        public IMainWindowUI MainWindowUI { get; set; }
        public IProfileUI ProfileUI { get; set; }
        public IHeroHomeUI HeroHomeUI { get; set; }
        public IDeckEditUI DeckEditUI { get; set; }
        public IValkyrieUI ValkyrieUI { get; set; }
        public IMerchantUI MerchantUI { get; set; }
        public ISmithUI SmithUI { get; set; }
        public ISeerUI SeerUI { get; set; }
        public ILonghouseUI LonghouseUI { get; set; }
        public ICityUI CityUI { get; set; }
        public ICreateHeroUI CreateHeroUI { get; set; }
        public IBattleBoardUI BattleBoardUI { get; set; }
        public IPreBattleUI PreBattleUI { get; set; }
        public IPostBattleUI PostBattleUI { get; set; }
        public ISurrenderUI SurrenderUI { get; set; }
        public IMapUI WorldMapUI { get; set; }
        public VikingSagaUserProfile Profile { get; set; }

        private static GameController _gameController;
        public static GameController Current
        {
            get
            {
                if (_gameController == null)
                    _gameController = new GameController();

                return _gameController;
            }
        }

        private GameController()
        {
            WorldMapUI = new WorldMapControl();
            ProfileUI = new ProfileHomeControl();
            HeroHomeUI = new HeroHomeControl();
            CreateHeroUI = new CreateHeroControl();
            BattleBoardUI = new BattleBoardControl();
            PreBattleUI = new PreBattleWindow();
            PostBattleUI = new PostBattleWindow();
            SurrenderUI = new SurrenderWindow();
            CityUI = new CityControl();
            DeckEditUI = new EditDeckControl();
            ValkyrieUI = new ValkyrieControl();
            MerchantUI = new MerchantControl();
            SeerUI = new SeerControl();
            LonghouseUI = new LonghouseControl();
            SmithUI = new SmithHouseControl();
            
            GameEngine.Current.BattleLostEvent += BattleLostEvent;
            GameEngine.Current.BattleWonEvent += BattleWonEvent;
            GameEngine.Current.LevelGainedEvent+= LevelGainedEvent;
        }

        private void LevelGainedEvent()
        {
            PostBattleUI.ShowLevelGained(Profile.SelectedHero);
        }

        private void BattleWonEvent(Encounter encounter, MapLocation oldMapLocation, MapLocation newMapLocation)
        {
            BattleWonEventHandler(encounter, oldMapLocation, newMapLocation);
        }

        private void BattleLostEvent(Encounter encounter)
        {
            BattleLostEventHandler(encounter);
        }

        private void BattleLostEventHandler(Encounter encounter)
        {
            MainWindowUI.EnableNonCombatButtons();
            PostBattleUI.ShowYouLostScreen(encounter);
            BattleBoardUI.CloseBattleBoard();
            ShowPreviousWindow();
        }

        private void BattleWonEventHandler(Encounter encounter, MapLocation oldMapLocation, MapLocation newMapLocation)
        {
            //var oldMapLocation = GameEngine.Current.PendingLocation;

            DAC.StoreProfile(Profile);

            if (GameEngine.Current.PendingLocation != null) // Not a random arena battle                
                WorldMapUI.DrawNewHeroMapPosition(oldMapLocation, newMapLocation);

            MainWindowUI.EnableNonCombatButtons();
            PostBattleUI.ShowYouWonScreen(encounter);

            BattleBoardUI.CloseBattleBoard();
            ShowMap(); // TODO, Different if coming from a random arena battle
        }        

        internal void StartRandomBattle()
        {
            GameEngine.Current.PendingLocation = null;
            var encounter = EncounterFactory.GetRandomEncounter(new Random());
            if (encounter.PlayableCards.Count <= 0)
                throw new Exception("The enemy has no cards"); // Could be OK actually

            PreBattle(encounter);
        }

        private void PreBattle(Encounter encounter)
        {
            GameEngine.Current.PendingEncounter = encounter;
            PreBattleUI.Show(encounter);
            //MainWindow.BodyContent.Content = (System.Windows.Controls.UserControl)PreBattleUI;
        }

        internal void StartBattle()
        {
            MainWindowUI.DisableNonCombatButtons();
            ShowBattleBoard();

            GameEngine.Current.StartNewBattle(Profile, BattleController);
        }

        private void ShowBattleBoard()
        {
            BattleBoardUI.ShowBattleBoard();
            MainWindowUI.ChangeBodyContent(BattleBoardUI);
        }

        private void ShowPreviousWindow()
        {
            ShowProfile(); // TODO
        }

        internal void ShowProfile()
        {
            ProfileUI.UpdateProfileDetails(Profile);
            MainWindowUI.ChangeBodyContent(ProfileUI);
        }

        internal void ShowSelectedHero()
        {
            HeroHomeUI.Update(Profile.SelectedHero, Profile.Deck); // TODO
            MainWindowUI.ChangeBodyContent(HeroHomeUI);
        }

        internal void ShowCreateHero()
        {
            CreateHeroUI.Update(Profile);
            MainWindowUI.ChangeBodyContent(CreateHeroUI);
        }

        internal void ShowMap()
        {
            WorldMapUI.DrawMap(Profile.SelectedHero.Map, Profile.SelectedHero); // TODO should be moved to constructor
            WorldMapUI.MarkHeroLocation(Map.GetMapLocation(Profile.SelectedHero.Map, Profile.SelectedHero.Map.HeroCoordinates.X, Profile.SelectedHero.Map.HeroCoordinates.Y));

            MainWindowUI.ChangeBodyContent(WorldMapUI);
        }

        internal void ShowVillage()
        {
            MainWindowUI.ChangeBodyContent(CityUI);
        }

        internal void CreateHero(String heroName, string selectedHeroClass)
        {
            GameEngine.Current.CreateHero(Profile, heroName, selectedHeroClass);
        }

        // Should be TryMapPositionChanged or MapPositionChanging
        internal void MapPositionChanged(MapLocation newMapLocation)
        {
            if (GameEngine.Current.IsNewPositionConnected(Profile.SelectedHero, newMapLocation))
            {
                GameEngine.Current.PendingLocation = newMapLocation;
                PreBattle(newMapLocation.Encounter);
            }
            else
            {
                WorldMapUI.ShowInvalidHeroMoveEffect(newMapLocation);
            }
        }

        internal void EngageEncounter()
        {
            PreBattleUI.Hide();
            GameController.Current.StartBattle();
        }

        internal void ExitGame()
        {
            MainWindowUI.Close();
        }

        internal void FleeFromEncounter()
        {
            PreBattleUI.Hide();
        }

        internal void PrepareSurrender()
        {
            SurrenderUI.Show();
        }

        internal void SurrenderNow()
        {
            GameEngine.Current.Surrender();
            MainWindowUI.EnableNonCombatButtons();
            ShowMap();
            SurrenderUI.Hide();
        }

        internal void CancelSurrender()
        {
            SurrenderUI.Hide();
        }

        //void IGameEngineObserver.YourHeroAttacked(Encounter encounter, Card activeCard, Hero.ConditionEnum condition, Hero hero)
        //{
        //    SequentialActions.RunBlocking(BattleBoardUI.ShowYourHeroAttackedEffect(activeCard, condition));
        //    SequentialActions.RunBlocking(BattleBoardUI.UpdateYourHeroControl(hero));
        //}



        internal void CreateNewHero(string heroName, Type heroType, int heroIndex)
        {
            if (heroType == typeof(Warrior))
            {
                var cardImageURL = @"heroes\warrior-hero.png";
                var map = MapFactory.CreateMap(MapFactory.MapEnum.DefaultWorld);
                var campaign = Campaign.CampaignFactory.CreateCampaign(Campaign.CampaignFactory.CampaignEnum.TheBloodWolf);
                Hero hero = new Warrior { Name = heroName, HP = 10, Mana = 5, Level = 1, XP = 0, Gold = 0, CardImageURL = cardImageURL, Map = map, Campaign = campaign };
                Profile.Heroes[heroIndex] = hero;

                DAC.StoreProfile(Profile);
            }
            else
                throw new Exception("Unknown hero type [" + heroType.ToString() + "]");
        }

        internal void ReviveCard(Card card)
        {
            GameEngine.Current.ReviveCard(card);
        }

        internal void BuyCard(Card card)
        {
            GameEngine.Current.BuyCard(card);
        }

        internal void ShowLonghouse()
        {            
            LonghouseUI.Show(Profile.SelectedHero, Profile.Deck);
            MainWindowUI.ChangeBodyContent(LonghouseUI);
        }

        internal void ShowSmithHouse()
        {
            SmithUI.Show(Profile.SelectedHero, Profile.Deck);
            MainWindowUI.ChangeBodyContent(SmithUI);
        }

        internal void ShowSeerHut()
        {
            SeerUI.Show(Profile.SelectedHero, Profile.Deck);
            MainWindowUI.ChangeBodyContent(SeerUI);
        }

        internal void ShowValkyrieGraveyard()
        {
            ValkyrieUI.Show(Profile.SelectedHero, Profile.Deck);
            MainWindowUI.ChangeBodyContent(ValkyrieUI);
        }

        internal void ShowMerchantShop()
        {
            MerchantUI.Show(Profile.SelectedHero, Profile.Deck);
            MainWindowUI.ChangeBodyContent(MerchantUI);
        }
    }
}
