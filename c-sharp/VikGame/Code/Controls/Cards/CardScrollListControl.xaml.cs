using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using Vik.Code.Game.Main.Profiles;
using Vik.Code.Controls.Utility;
using System;
using System.Windows.Media;
using GameLib.Battles.Cards;
using System.Windows;
using System.ComponentModel;
using Vik.Code.Utility;
using System.Windows.Threading;

namespace Vik.Code.Controls.Cards
{
    public partial class CardScrollList : UserControl, IDisposable
    {
        public CardBattle.CardFlagsEnum DragDropAcceptFlags { get; set; }

        private CardBattle.CardFlagsEnum _displayFlags;
        private double _cardW;
        private const double CardMarginX = 10;
        private const double CardMarginY = 10;
        private List<string> _cardIds = new List<string>();

        public CardScrollList()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _defaultScrollBackgroundBrush = Scroll.Background;

            _displayFlags = CardBattle.CardFlagsEnum.Null;
            DragDropAcceptFlags = CardBattle.CardFlagsEnum.All;

            this.IsVisibleChanged += CardScrollList_IsVisibleChanged;
        }

        [Description("Raised when a card is clicked."), Category("Viking")]
        public event EventHandler CardClicked;

        void CardScrollList_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;
            if (value == true)
            {
                VikGame.ScreenManager.DragDropManager.CardDrag += DragDropManager_CardDrag;
            }
            else
            {
                VikGame.ScreenManager.DragDropManager.CardDrag -= DragDropManager_CardDrag;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                VikGame.ScreenManager.DragDropManager.CardDrag -= DragDropManager_CardDrag;
            }
        }

        public void SetDisplayFlags(CardBattle.CardFlagsEnum flags, bool refresh = false)
        {
            _displayFlags = flags;
            if (refresh)
                Refresh();
        }

        private Brush _defaultScrollBackgroundBrush;

        private void RemoveHighlightDeny()
        {
            if (this.Border.BorderBrush != Brushes.Transparent)
                this.Border.BorderBrush = Brushes.Transparent;

            if (this.Scroll.Background != _defaultScrollBackgroundBrush)
                this.Scroll.Background = _defaultScrollBackgroundBrush;
        }

        private void Highlight(bool half)
        {
            if (this.Border.BorderBrush != UiUtil.DragDropBorderHighlightBrush)
                this.Border.BorderBrush = UiUtil.DragDropBorderHighlightBrush;

            var background = half ? _defaultScrollBackgroundBrush : UiUtil.DragDropBackgroundHighlightBrush;
            if (this.Scroll.Background != background)
                this.Scroll.Background = background;
        }

        private void Deny()
        {
            this.Border.BorderBrush = UiUtil.DragDropBorderDenyBrush;
        }

        void DragDropManager_CardDrag(object sender, CardDragEventArgs args)
        {
            if (sender == this)
            {
                MyCardDraggedOnOther(args);
            }
            else
            {
                CardDraggedOnMe(args);
            }
        }

        void MyCardDraggedOnOther(CardDragEventArgs args)
        {
            if (args.DragEvent == CardDragEventType.DropQueryAccept && args.Accept == true)
            {
                int idx = this.Canvas.Children.IndexOf(args.OwningPlaceholder);
                if (idx >= 0)
                {
                    // Card being dropped is owned by a placeholder in this scroll control.
                    _cardIds.Remove(args.Card.GetId());
                    Refresh();
                }
            }
        }

        private void Refresh()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action)RefreshAsync, DispatcherPriority.ContextIdle);
        }

        void CardDraggedOnMe(CardDragEventArgs args)
        {
            if (args.DragEvent == CardDragEventType.Dropped)
            {
                RemoveHighlightDeny();
                return;
            }

            var accept = args.Card.HasAnyFlag(this.DragDropAcceptFlags);
            if (!accept)
            {
                if (args.DragEvent == CardDragEventType.DragBegin)
                    Deny();
                else if (args.DragEvent == CardDragEventType.DropQueryAccept)
                    RemoveHighlightDeny();

                return;
            }

            var thisRect = UiUtil.GetWindowSpaceRect(this);

            var overlap = Util.Overlap(args.DragRect, thisRect);
            var isBeginDrag = args.DragEvent == Code.Utility.CardDragEventType.DragBegin;
            var isMoveDrag = args.DragEvent == Code.Utility.CardDragEventType.DragMove;
            var isDrop = args.DragEvent == Code.Utility.CardDragEventType.DropQueryAccept;

            Highlight(!overlap);

            if (isDrop)
            {
                RemoveHighlightDeny();

                if (overlap)
                {
                    CardDroppedOnMe(args);
                }
            }
        }

        public void AddCard(string cardId, bool refresh = false)
        {
            _cardIds.Add(cardId);
        }

        void CardDroppedOnMe(CardDragEventArgs args)
        {
            AddCard(args.Card.GetId());

            args.Accept = true;
            args.AcceptingElement = Scroll;

            Refresh();
        }

        public IEnumerable<string> GetCardIds()
        {
            return _cardIds;
        }

        public void SetCardsIds(IEnumerable<string> cardIds, bool refresh = false)
        {
            _cardIds.Clear();
            _cardIds.AddRange(cardIds);

            if (refresh)
                Refresh();
        }

        public void RefreshAsync()
        {
            var filteredCards = CardBattle.FilterCards(_cardIds, _displayFlags).ToList();
            int count = filteredCards.Count;

            tbCardCount.Text = string.Format("{0} Card{1}", count, count == 1 ? string.Empty : "s");

            const double cardMarginX = 5;

            double cardHeight = Math.Max(this.Canvas.ActualHeight - (2 * CardMarginY), 10);
            _cardW = Math.Max(cardHeight / UiUtil.CardAspectRatio, 20);

            double canvasWidth = (count * (_cardW + cardMarginX)) + cardMarginX;
            this.Canvas.Width = canvasWidth;

            this.Canvas.Children.Clear();

            for (int i = 0; i < count; ++i)
            {
                var ph = new CardPlaceholderControl();
                ph.DragDropParent = this;
                ph.Width = _cardW;
                ph.Height = cardHeight;
                Canvas.SetTop(ph, CardMarginY);
                double x = ((_cardW + CardMarginX) * i) + CardMarginX;
                Canvas.SetLeft(ph, x);
                ph.SetCard(filteredCards[i]);
                var cardControl = ph.GetCardControl();
                cardControl.MouseLeftButtonDown += cardControl_MouseLeftButtonDown;
                ph.SetIsDraggable(true);

                this.Canvas.Children.Add(ph);
            }
        }

        void cardControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CardClicked != null)
                CardClicked(sender, EventArgs.Empty);
        }
    }
}
