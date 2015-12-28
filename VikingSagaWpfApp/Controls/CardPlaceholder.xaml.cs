using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VikingSaga.Code;
using VikingSagaWpfApp.Code.BattleNs;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp
{
    public partial class CardPlaceholder : UserControl
    {
        public CardPlaceholder()
        {
            InitializeComponent();
            IsSelected = false;
        }

        public void ClearCardControl()
        {
            _cardControl = null;
        }

        // Move cardControl from one placeholder into an empty placeholder.
        public void MoveCardControl(CardControl cardControl)
        {
            if (this.CardControl == cardControl)
                return;

            var owner = cardControl.OwningPlaceholder;
            if (owner != null)
                owner.ClearCardControl();

            SetCardControl(cardControl);
        }

        // Place cardControl into an empty placeholder. Must not already be in a placeholder.
        private void SetCardControl(CardControl cardControl)
        {
            if (_cardControl != null)
                throw new InvalidOperationException("This CardPlaceHolder already contains a card");

            cardControl.Position = CardPlaceholderIndex;
            cardControl.OwningPlaceholder = this;

            if (!this.IsHand)
            {
                cardControl.IsDraggable = false;
            }

            _cardControl = cardControl;
        }

        public void DropCardOnOtherCard(CardControl cardControl)
        {
            var card = cardControl.Card;
            if (card is CardInstant)
            {
                var player = card.Owner;
                Task.Run(() =>
                    {
                        // This must not be called on UI thread, see SequentialActions.RunBlocking
                        player.DropCardOtherCard((CardInstant)card, (CardBasicMob)this.CardControl.Card, false);
                    });
            }
        }

        public void ShowReadyForDrop(bool show)
        {
            Border.BorderBrush = show ? Brushes.Green : Brushes.Black;
        }

        public void DropOnEmptyBoardPlaceHolder(CardControl cardControl)
        {
            if (this.CardControl == cardControl)
                return;

            var card = cardControl.Card;
            if (card is CardBasicMob)
            {
                var player = card.Owner;
                Task.Run(() =>
                    {
                        // This must not be called on UI thread, see SequentialActions.RunBlocking
                        player.DropCardOnBoard((CardBasicMob)card, card.HandPosition, this.CardPlaceholderIndex, false);
                    });
            }
        }

         public bool CanReceiveDroppedCard(CardControl cardControl)
        {
            BattleCard bc = cardControl.Card;

            if (IsHand)
                return false;

            bool targetOwnCardOk = bc.CanTargetOwnCard && IsYours && IsOccupied;
            bool targetOwnBoardOk = bc.CanTargetOwnBoard && IsYours && !IsOccupied;
            bool targetEnemyCardOk = bc.CanTargetEnemyCard && !IsYours && IsOccupied;

            bool result = targetOwnCardOk || targetOwnBoardOk || targetEnemyCardOk;
            return result;
        }

        private CardControl _cardControl;
        public CardControl CardControl { get { return _cardControl; } }

        [Category("VikingSaga"), Description("Is the placeholder occupied already.")]
        public bool IsOccupied
        {
            get { return _cardControl != null; }
        }

        private bool _isYours;
        [Category("VikingSaga"), Description("Is the placeholder yours.")]
        public bool IsYours
        {
            get { return _isYours; }
            set { _isYours = value; }
        }

        private int _cardPlaceholderIndex;
        [Category("VikingSaga"), Description("The index of the card placeholder.")]
        public int CardPlaceholderIndex
        {
            get { return _cardPlaceholderIndex; }
            set { _cardPlaceholderIndex = value; }
        }

        public bool IsHand { get; set; }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                if (_isSelected)
                    SelectCard();
                else
                    DeSelectCard();
            }
        }

        private void SelectCard()
        {
            Dispatcher.Invoke(() =>
            {
                SelectionBorderImage.Visibility = System.Windows.Visibility.Visible;
            });
        }

        private void DeSelectCard()
        {
            Dispatcher.Invoke(() =>
            {
                SelectionBorderImage.Visibility = System.Windows.Visibility.Hidden;
            });
        }

    }
}
