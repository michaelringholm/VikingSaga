using GameLib.Battles.Cards;
using GameLib.DataStore.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vik.Code.Controls.Cards;
using Vik.Code.Controls.Utility;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Player
{
    public partial class PlayerEditPartyControl : UserControl
    {
        private List<CardFilterControl> cardFilters = new List<CardFilterControl>();
        private CardPlaceholderControl[] _placeholders = new CardPlaceholderControl[5];

        public PlayerEditPartyControl()
        {
            InitializeComponent();

            if(!DesignerProperties.GetIsInDesignMode(this))
                this.Background = new SolidColorBrush(Colors.Transparent);

            InitPlaceholders();

            IsVisibleChanged += PlayerEditPartyControl_IsVisibleChanged;
            Loaded += PlayerEditPartyControl_Loaded;

            SetCardsFromProfile(VikGame.World.PlayerProfile.Data);
        }

        private void InitPlaceholders()
        {
            _placeholders[0] = this.ph0;
            _placeholders[1] = this.ph1;
            _placeholders[2] = this.ph2;
            _placeholders[3] = this.ph3;
            _placeholders[4] = this.ph4;
       }

        void PlayerEditPartyControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var visible = (bool)e.NewValue;
            if (visible)
            {
                VikGame.ScreenManager.DragDropManager.CardDrag += DragDropManager_CardDrag;
            }
            else
            {
                VikGame.ScreenManager.DragDropManager.CardDrag -= DragDropManager_CardDrag;
            }
        }

        //private CardPlaceholderControl _partyDragSource = null;

        void DragDropManager_CardDrag(object sender, Code.Utility.CardDragEventArgs e)
        {
            if (sender == partyRemainingCardsScrollList)
                FromCardScrollListDragDrop(e);
            else
                FromPartyCardDragDrop(e);
        }

        // Returns overlapped party placeholder, if any
        private CardPlaceholderControl GetOverlappedPartyPlaceholder(CardDragEventArgs e)
        {
            foreach (var ph in _placeholders)
            {
                var thisRect = UiUtil.GetWindowSpaceRect(ph);
                var overlap = Util.Overlap(e.DragRect, thisRect);
                if (overlap)
                    return ph;
            }
            return null;
        }

        private void ClearPlaceholderHighlight()
        {
            foreach (var ph in _placeholders)
                ph.RemoveHighlight();
        }

        private void OnDrag(CardDragEventArgs e)
        {
            bool firstOverlapFound = false; // Only highlight first match. There can be more, depending on card placement, rotation, etc.
            foreach (var ph in _placeholders)
            {
                var thisRect = UiUtil.GetWindowSpaceRect(ph);
                var overlap = Util.Overlap(e.DragRect, thisRect);
                ph.HighlightAccept(!overlap || firstOverlapFound);
                firstOverlapFound |= overlap;
            }
        }

        private void FromPartyCardDragDrop(CardDragEventArgs e)
        {
            var partyPlaceholderDropTarget = GetOverlappedPartyPlaceholder(e);
            bool overPartyCard = partyPlaceholderDropTarget != null;

            if (e.DragEvent == CardDragEventType.DropQueryAccept)
            {
                if (!overPartyCard)
                    return;

                // Party to party
                bool dropOnSelf = e.OwningPlaceholder == partyPlaceholderDropTarget;
                if (dropOnSelf)
                    return;

                e.Accept = true;
                e.AcceptingElement = partyPlaceholderDropTarget;
            }
            else if (e.DragEvent == CardDragEventType.Dropped)
            {
                ClearPlaceholderHighlight();

                if (!e.Accept)
                    return;

                var srcId = e.Card.GetId();

                if (overPartyCard)
                {
                    var dstId = partyPlaceholderDropTarget.GetCardId();

                    // Party to party, swap cards
                    partyPlaceholderDropTarget.SetCard(srcId);
                    partyPlaceholderDropTarget.SetIsDraggable(true);

                    e.OwningPlaceholder.SetCard(dstId);
                    e.OwningPlaceholder.SetIsDraggable(true);
                }
                else
                {
                    // Party to scroll control. Scroll control handles itself, just remove card from party
                    e.OwningPlaceholder.ClearCardControl();
                }
            }
            else
            {
                OnDrag(e);
            }
        }

        private void FromCardScrollListDragDrop(CardDragEventArgs e)
        {
            var partyPlaceholderDropTarget = GetOverlappedPartyPlaceholder(e);
            if (e.DragEvent == CardDragEventType.DropQueryAccept)
            {
                if (partyPlaceholderDropTarget != null)
                {
                    // Drop from scroll control onto party
                    e.Accept = true;
                    e.AcceptingElement = partyPlaceholderDropTarget;

                    string existingPartyCardId = partyPlaceholderDropTarget.GetCardId();
                    if (!string.IsNullOrEmpty(existingPartyCardId))
                    {
                        // Drop onto existing party member, move it to the scroll control
                        partyRemainingCardsScrollList.AddCard(existingPartyCardId);
                    }

                    partyPlaceholderDropTarget.SetCard(e.Card);
                    partyPlaceholderDropTarget.SetIsDraggable(true);
                }
            }
            else if (e.DragEvent == CardDragEventType.Dropped)
            {
                ClearPlaceholderHighlight();
            }
            else
            {
                OnDrag(e);
            }
        }

        void PlayerEditPartyControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitToggleButtons();
        }

        public void SetCardsFromProfile(ProfileDTO profile)
        {
            partyRemainingCardsScrollList.SetCardsIds(profile.RemainingCards, refresh: true);

            for (int i = 0; i < 5; ++i)
            {
                var card = profile.PartyCards[i];
                var ph = _placeholders[i];
                ph.SetCard(card);
                ph.SetIsDraggable(true);
            }
        }

        public void ApplyCardsToProfile(ProfileDTO profile)
        {
            profile.RemainingCards = new List<string>(this.partyRemainingCardsScrollList.GetCardIds());
            for (int i = 0; i < 5; ++i)
            {
                profile.PartyCards[i] = _placeholders[i].GetCardId();
            }
        }

        private void UpdateScrollControlDisplayFlags(bool refresh)
        {
            var flags = CardFilter.BuildFlags(cardFilters);
            partyRemainingCardsScrollList.SetDisplayFlags(flags, refresh);
        }

        private void InitToggleButtons()
        {
            foreach(UIElement child in FiltersPanel.Children)
                if(child is CardFilterControl)
                    cardFilters.Add((CardFilterControl)child);

            UpdateScrollControlDisplayFlags(refresh: true);
        }

        private void CardFilterControl_FilterToggled(object sender, EventArgs e)
        {
            UpdateScrollControlDisplayFlags(refresh: true);
        }
    }
}
