using GameLib.Battles.Cards;
using GameLib.Game;
using GameLib.Quests;
using GameLib.World;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using Vik.Code.Controls.Cards;
using Vik.Code.Controls.Utility;
using Vik.Code.Game.Main.Interfaces;
using Vik.Code.Game.Main.Profiles;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Towns
{
    public partial class MerchantShopWindow : FakeWindow, ISpecialLocation
    {
        private MerchantShopWindow()
        {
            InitializeComponent();
            Init();
        }

        private WorldData.SpecialLocationEnum _id;
        public MerchantShopWindow(WorldData.SpecialLocationEnum id)
        {
            InitializeComponent();
            _id = id;
            Init();
        }

        private void Init()
        {
            VikGame.Sound.Play("Merchant/welcome.wav");
            this.Cursor = VikGame.ResourceLocator.GetCursorResource("coins.cur");
            CardControl.SetCard(new CardMerchant(), Cards.CardControl.StatDisplayFlags.None);
            UpdateCardScrollList();
        }

        private void UpdateCardScrollList()
        {
            var sellableCards = new List<string>();
            //sellableCards.AddRange(VikGame.World.PlayerProfile.Data.PartyCards);
            sellableCards.AddRange(Card.FilterCards(VikGame.World.PlayerProfile.Data.RemainingCards, Card.CardFlagsEnum.All));
            CardScrollList.SetCardsIds(sellableCards);
            CardScrollList.SetDisplayFlags(Card.CardFlagsEnum.All, true);
            CardScrollList.RefreshAsync();
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
            //VikGame.World.LeaveSpecialLocation();
            Close();
        }

        private void CardScrollList_CardClicked(object sender, System.EventArgs e)
        {
            var card = ((CardControl)sender).Card;
            if (card != null)
            {
                SellCard(card);                
            }
        }

        private void SellCard(Card card)
        {
            UiUtil.ShowFloatingInfo("Card [" + card.Name + "] sold!", 0.5, 0.3, true, Colors.Tomato);
            //PlayAsync("/Data/Sound/Merchant/coins.wav");
            VikGame.Sound.Play("Merchant/coins.wav");
            VikGame.World.PlayerProfile.SellCard(card);
            UpdateCardScrollList();
        }
    }
}
