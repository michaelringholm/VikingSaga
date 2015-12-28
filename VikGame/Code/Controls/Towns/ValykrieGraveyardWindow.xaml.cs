using GameLib.Battles.Cards;
using GameLib.Game;
using GameLib.Quests;
using GameLib.World;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using Vik.Code.Controls.Cards;
using Vik.Code.Controls.Utility;
using Vik.Code.Game.Main.Interfaces;
using Vik.Code.Game.Main.Profiles;
using System.Windows.Input;

namespace Vik.Code.Controls.Towns
{
    public partial class ValykrieGraveyardWindow : FakeWindow, ISpecialLocation
    {
        private ValykrieGraveyardWindow()
        {
            InitializeComponent();
            Init();
        }

        private WorldData.SpecialLocationEnum _id;
        public ValykrieGraveyardWindow(WorldData.SpecialLocationEnum id)
        {
            InitializeComponent();
            _id = id;
            Init();
        }

        private void Init()
        {
            this.Cursor = VikGame.ResourceLocator.GetCursorResource("spear.cur");
            CardControl.SetCard(new CardValkyrie(), Cards.CardControl.StatDisplayFlags.None);
            UpdateAllControls();
        }

        private void UpdateAllControls()
        {
            UpdateCardScrollList();
            UpdateReviveAllCosts();
        }

        private void UpdateReviveAllCosts()
        {
            var sum = getReviveAllCost();
            tbGoldCost.Text = sum.ToString();

            btnReviveAll.IsEnabled = sum <= VikGame.World.PlayerProfile.Data.Gold;

            if (btnReviveAll.IsEnabled)
                btnReviveAll.Cursor = VikGame.ResourceLocator.GetCursorResource("spear.cur");
            else
                btnReviveAll.Cursor = Cursors.Cross;
        }

        private int getReviveAllCost()
        {
            return Card.CardsFromIds(GetFollowerAndMinionCardIds()).Where(c => c.NeedRevive).Sum(c => c.ReviveCost);
        }

        private void UpdateCardScrollList()
        {
            var filteredCards = GetFollowerAndMinionCardIds();
            CardScrollList.SetCardsIds(filteredCards);
            CardScrollList.SetDisplayFlags(Card.CardFlagsEnum.Follower | Card.CardFlagsEnum.Minion, true);
        }

        private IEnumerable<string> GetFollowerAndMinionCardIds()
        {
            var followerAndMinionCards = new List<string>();
            followerAndMinionCards.AddRange(VikGame.World.PlayerProfile.Data.PartyCards);
            followerAndMinionCards.AddRange(VikGame.World.PlayerProfile.Data.RemainingCards);
            var filteredCards = Card.FilterCards(followerAndMinionCards, Card.CardFlagsEnum.Follower | Card.CardFlagsEnum.Minion);
            return filteredCards;
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

        private void ReviveAll(object sender, System.Windows.RoutedEventArgs e)
        {
            ReviveAll();
        }

        private void ReviveAll()
        {
            var filteredCards = Card.FilterCards(GetFollowerAndMinionCardIds(), Card.CardFlagsEnum.Follower | Card.CardFlagsEnum.Minion);

            if (Card.CardsFromIds(filteredCards).Count(c => c.NeedRevive) <= 0)
            {
                UiUtil.ShowFloatingInfo("All cards are in good shape, no need to revive any of them!", 0.5, 0.3, true, Colors.Tomato);
                return;
            }

            var sum = getReviveAllCost();

            if (sum <= VikGame.World.PlayerProfile.Data.Gold)
            {
                VikGame.Sound.Play("Valkyrie/revive.wav");
                VikGame.World.PlayerProfile.ReviveAll();
                VikGame.World.PlayerProfile.Data.Gold -= sum;
                UpdateAllControls();
            }            
        }

        private void CardScrollList_CardClicked(object sender, System.EventArgs e)
        {
            var card = ((CardControl)sender).Card;
            if (card != null)
            {
                ReviveCard(card);
            }
        }

        private void ReviveCard(Card card)
        {
            if(!card.NeedRevive)
            {
                UiUtil.ShowFloatingInfo("This card is in good shape, no need to revive!", 0.5, 0.3, true, Colors.Tomato);
                return;
            }

            if (card.ReviveCost <= VikGame.World.PlayerProfile.Data.Gold)
            {
                VikGame.World.PlayerProfile.SetNeedReviveFlag(card.GetId(), false);
                VikGame.World.PlayerProfile.Data.Gold -= card.ReviveCost;
                UiUtil.ShowFloatingInfo("Card [" + card.Name + "] has been revived!", 0.5, 0.3, true, Colors.Tomato);
                VikGame.Sound.Play("Valkyrie/revive.wav");
                UpdateAllControls();
            }
            else
            {
                UiUtil.ShowFloatingInfo("Not enough gold!", 0.5, 0.3, true, Colors.Tomato);
            }            
        }
    }
}
