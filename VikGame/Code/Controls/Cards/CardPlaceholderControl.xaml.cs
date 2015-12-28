using GameLib.Battles.Cards;
using System;
using System.Collections.Generic;
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
using Vik.Code.Controls.Utility;

namespace Vik.Code.Controls.Cards
{
    public partial class CardPlaceholderControl : UserControl
    {
        public FrameworkElement DragDropParent { get; set; }

        public int CardPlaceholderIndex { get; set; }
        public bool IsHand { get; set; }
        public bool IsYours { get; set; }

        private Brush _defaultBorderBrush;
        private Brush _defaultBackgroundBrush;

        public CardPlaceholderControl()
        {
            InitializeComponent();

            _defaultBorderBrush = this.HighlightBorder.BorderBrush;
            _defaultBackgroundBrush = this.HighlightBorder.Background;
        }

        public string GetCardId()
        {
            var cardControl = GetCardControl();
            return cardControl == null ? string.Empty : cardControl.Card.GetId();
        }

        public void SetIsDraggable(bool isDraggable)
        {
            var cardControl = GetCardControl();
            if (cardControl != null)
                cardControl.IsDraggable = isDraggable;
        }

        public Card GetInnerCard()
        {
            return GetCardControl() == null ? null : GetCardControl().Card;
        }

        public CardControl GetCardControl()
        {
            return this.CardControlContainer.Child as CardControl;
        }

        public void SetCard(string cardId)
        {
            var card = CardBattle.CardFromId(cardId);
            SetCard(card);
        }

        public void SetCard(Card card)
        {
            CardControl cardControl = card == null ? null : new CardControl(card, Cards.CardControl.StatDisplayFlags.All);
            SetCardControl(cardControl);
            this.CardControlContainer.Child = cardControl;
        }

        public void SetCardControl(CardControl cardControl)
        {
            if (cardControl != null)
            {
                cardControl.SetOwner(this, DragDropParent ?? this);
            }
            this.CardControlContainer.Child = cardControl;
        }

        public void ClearCardControl()
        {
            this.CardControlContainer.Child = null;
        }

        public void RemoveHighlight()
        {
            if (this.HighlightBorder.BorderBrush != Brushes.Transparent)
                this.HighlightBorder.BorderBrush = Brushes.Transparent;

            if (this.HighlightBorder.Background != _defaultBackgroundBrush)
                this.HighlightBorder.Background = _defaultBackgroundBrush;
        }

        public void HighlightAccept(bool half)
        {
            if (this.HighlightBorder.BorderBrush != UiUtil.DragDropBorderHighlightBrush)
                this.HighlightBorder.BorderBrush = UiUtil.DragDropBorderHighlightBrush;

            var background = half ? _defaultBackgroundBrush : UiUtil.DragDropBackgroundHighlightBrush;
            if (this.HighlightBorder.Background != background)
                this.HighlightBorder.Background = background;
        }

        public void HighlightDeny()
        {
            if (this.HighlightBorder.BorderBrush != UiUtil.DragDropBorderDenyBrush)
                this.HighlightBorder.BorderBrush = UiUtil.DragDropBorderDenyBrush;
        }
    }
}
