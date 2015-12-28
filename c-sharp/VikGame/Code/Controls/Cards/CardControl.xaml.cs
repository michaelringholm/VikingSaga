using System.Windows.Controls;
using GameLib.Battles.Cards;
using Vik.Code.Controls.Utility;
using System.Windows;
using Vik.Code.Utility;
using System.ComponentModel;
using System;
using System.Windows.Media;

namespace Vik.Code.Controls.Cards
{
    public partial class CardControl : UserControl
    {
        [Flags]
        public enum StatDisplayFlags
        {
            None = 0,
            Level = 1 << 0,
            Power = 1 << 1,
            Hp = 1 << 2,
            All = -1
        };

        public Card Card { get; private set; }
        public bool IsZoomable { get; set; }
        public bool IsDraggable { get; set; }
        public bool IsAligning { get { return _dragHelper == null ? _battleCardHelper.IsAligning : _dragHelper.IsAligning; } }
        public bool IsSelected { get; set; }
        public int Position { get; set; }
        public CardPlaceholderControl OwningPlaceholder { get; private set; }
        public FrameworkElement DragDropParent { get; private set; }

        public StatDisplayFlags DisplayFlags { get; private set; }

        private CardDragHelper _dragHelper;
        private BattleCardHelper _battleCardHelper;

        public CardControl()
        {
            Init(forBattle: false);
        }

        public CardControl(bool forBattle = false)
        {
            Init(forBattle);
        }

        public CardControl(Card card, StatDisplayFlags statDisplayFlags)
        {
            Init(forBattle: false);

            SetCard(card, statDisplayFlags);
        }

        private void Init(bool forBattle)
        {
            InitializeComponent();

            if (forBattle)
            {
                _battleCardHelper = new BattleCardHelper();
            }
            else
            {
                _dragHelper = new CardDragHelper();
            }
        }

        public void SetTranslateXY(double x, double y)
        {
            if (_battleCardHelper != null)
            {
                _battleCardHelper.SetTranslateXY(x, y);
            }
        }

        public void AlignToElement(FrameworkElement element, int ms)
        {
            if (_dragHelper != null)
                _dragHelper.AlignToElement(element, ms);
            if (_battleCardHelper != null)
                _battleCardHelper.AlignToElement(element, ms);
        }

        public void SetOwner(CardPlaceholderControl owningPlaceholder, FrameworkElement dragDropParent)
        {
            OwningPlaceholder = owningPlaceholder;
            DragDropParent = dragDropParent;

            if (_dragHelper != null)
            {
                _dragHelper.Init(this);
                _dragHelper.DragDropParent = dragDropParent;
            }
            else if (_battleCardHelper != null)
            {
                _battleCardHelper.Init(this);
            }
        }

        public void BringToFront()
        {
            if (this.OwningPlaceholder == null || this.OwningPlaceholder.Parent == null)
                return;

            var grid = (Panel)this.OwningPlaceholder.Parent;
            grid.Children.Remove(this.OwningPlaceholder);
            grid.Children.Add(this.OwningPlaceholder);
        }

        public void UpdateStats()
        {
            if (Card is CardAbility)
            {
                var abilityCard = Card as CardAbility;
                tbLevel.Text = abilityCard.Level.ToString();
                tbPower.Text = abilityCard.PowerCost.ToString();
            }
            else if (Card is CardBasicMob)
            {
                var mobCard = Card as CardBasicMob;
                tbLevel.Text = mobCard.Level.ToString();
                tbPower.Text = mobCard.Power.ToString();
                tbHp.Text = mobCard.Hp.ToString();
            }
        }

        internal void SetCard(Card card, StatDisplayFlags statDisplayFlags)
        {
            DisplayFlags = statDisplayFlags;

            this.Card = card;

            this.tbLevel.Visibility = DisplayFlags.HasFlag(StatDisplayFlags.Level) ? Visibility.Visible : Visibility.Hidden;
            this.tbPower.Visibility = DisplayFlags.HasFlag(StatDisplayFlags.Power) ? Visibility.Visible : Visibility.Hidden;
            this.tbHp.Visibility = DisplayFlags.HasFlag(StatDisplayFlags.Hp) ? Visibility.Visible : Visibility.Hidden;

            this.tbLevel.Text = card.Level.ToString();
            this.CardName.Text = card.Name;
            this.CardDescription.Text = card.Description;

            UpdateStats();
            UpdateBackgrondColors(card);

            if (!string.IsNullOrEmpty(card.ImageUrl))
            {
                var image = VikGame.ResourceLocator.GetImageResource(card.ImageUrl, throwOnError: false);
                if (image == null)
                    image = VikGame.ResourceLocator.GetImageResource(@"Data/Gfx/Cards/Abilities/healing-potion.jpg"); // Default image for now

                this.CardImage.Source = image;
            }

            if (card.NeedRevive)
                tbIsDead.Visibility = System.Windows.Visibility.Visible;
            else
                tbIsDead.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateBackgrondColors(GameLib.Battles.Cards.Card card)
        {
            Brush brush = (Brush)FindResource("UncommonCardColorBrush");
            Brush borderBrush = (Brush)FindResource("UncommonCardBorderColorBrush");
            if (card.Rarity == GameLib.Battles.Cards.Card.CardRarity.Common)
            {
                brush = (Brush)FindResource("UncommonCardColorBrush");
                borderBrush = (Brush)FindResource("UncommonCardBorderColorBrush");
            }
            else if (card.Rarity == GameLib.Battles.Cards.Card.CardRarity.Rare)
            {
                brush = (Brush)FindResource("RareCardColorBrush");
                borderBrush = (Brush)FindResource("RareCardBorderColorBrush");
            }
            else if (card.Rarity == GameLib.Battles.Cards.Card.CardRarity.Epic)
            {
                brush = (Brush)FindResource("EpicCardColorBrush");
                borderBrush = (Brush)FindResource("EpicCardBorderColorBrush");
            }

            TopCardBorder.Background = brush;
            BottomCardBorder.Background = brush;
            TopCardBorder.BorderBrush = borderBrush;
            MiddleCardBorder.BorderBrush = borderBrush;
            BottomCardBorder.BorderBrush = borderBrush;
        }
    }
}
