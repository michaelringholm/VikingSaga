using GameLib.Battles.Cards;
using GameLib.Game;
using GameLib.Quests;
using GameLib.World;
using Vik.Code.Controls.Cards;
using Vik.Code.Game.Main.Interfaces;
using Vik.Code.Game.Main.Profiles;

namespace Vik.Code.Controls.Towns
{
    public partial class LargeVillageWindow : FakeWindow, ISpecialLocation
    {
        private LargeVillageWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            VikGame.Sound.Play("Village/village.wav");
            ValkyrieCardControl.SetCard(new CardValkyrie(), Cards.CardControl.StatDisplayFlags.All);
            MerchantCardControl.SetCard(new CardMerchant(), Cards.CardControl.StatDisplayFlags.All);
            BarmaidCardControl.SetCard(new CardBarmaid(), Cards.CardControl.StatDisplayFlags.All);
            SeerCardControl.SetCard(new CardSeer(), Cards.CardControl.StatDisplayFlags.All);
            SmithCardControl.SetCard(new CardSmith(), Cards.CardControl.StatDisplayFlags.All);
        }

        private WorldData.SpecialLocationEnum _id;
        public LargeVillageWindow(WorldData.SpecialLocationEnum id)
        {
            InitializeComponent();
            _id = id;
            Init();
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

        private void NPC_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(sender != null && sender is CardControl)
            {
                var cardControl = (CardControl)sender;
                if (cardControl.Card != null && cardControl.Card is CardValkyrie)
                {
                    var window = new ValykrieGraveyardWindow(WorldData.SpecialLocationEnum.ValkyrieGraveyard);
                    VikGame.ScreenManager.ShowContentModal(window, window, true);
                }
                else if (cardControl.Card != null && cardControl.Card is CardMerchant)
                {
                    var window = new MerchantShopWindow(WorldData.SpecialLocationEnum.ValkyrieGraveyard);
                    VikGame.ScreenManager.ShowContentModal(window, window, true);
                }
            }
        }
    }
}
