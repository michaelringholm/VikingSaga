using GameLib.Battles.Cards;
using GameLib.Game;
using GameLib.Quests;
using GameLib.World;
using Vik.Code.Game.Main.Interfaces;
using Vik.Code.Game.Main.Profiles;

namespace Vik.Code.Controls.Towns
{
    public partial class SeersHutWindow : FakeWindow, ISpecialLocation
    {
        private SeersHutWindow()
        {
            InitializeComponent();
            InitCards();
        }

        private void InitCards()
        {
            CardControl.SetCard(new CardSeer(), Cards.CardControl.StatDisplayFlags.All);
        }

        private WorldData.SpecialLocationEnum _id;
        public SeersHutWindow(WorldData.SpecialLocationEnum id)
        {
            InitializeComponent();
            _id = id;
            InitCards();
        }

        void ISpecialLocation.Enter()
        {
            VikGame.World.GameEventManager.OnSpecialLocationEnteredEvent(this, new SpecialLocationEventArgs { Id = _id });
        }

        void ISpecialLocation.Leave()
        {
            VikGame.World.GameEventManager.OnSpecialLocationLeftEvent(this, new SpecialLocationEventArgs { Id = _id });
        }

        private void ButtonLeave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VikGame.World.LeaveSpecialLocation();
        }
    }
}
