using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VikingSaga.Code;
using VikingSagaWpfApp.Animations;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSagaWpfApp
{
    public partial class CardControl : UserControl
    {
        private DragHelper _drag = new DragHelper();

        public CardControl()
        {
            InitializeComponent();

            this.AllowDrop = true;
            IsSelected = false;
            HP.Text = string.Empty;
            Attack.Text = string.Empty;
        }

        public void InitDragHelper()
        {
            _drag.Init(this);
        }

        public bool IsZoomable { get; set; }
        public bool IsAligning { get { return _drag.IsAligning; } }

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

        private bool _isDead = false;
        public bool IsDead
        {
            get
            {
                return _isDead;
            }

            set
            {
                _isDead = value;
                if (_isDead)
                    ShowDeathOverlay();
                else
                    RemoveDeathOverlay();
            }
        }

        public BattleCard Card { get; set; }
        public bool IsDraggable { get; set; }
        public int Position { get; set; }
        public CardPlaceholder OwningPlaceholder { get; set; }

        public void AlignToUserControl(UserControl control, int ms)
        {
            _drag.AlignToUserControl(control, ms);
        }

        public IEnumerable<int> CardAttackedEffectActions()
        {
            var rip = EffectHelper.ApplyRipEffect(this.MainGrid);
            SoundUtil.PlaySound(SoundUtil.SoundEnum.RipCard);
            UpdateCardControl(this.Card);
            yield return 500;
            SoundUtil.PlaySound(SoundUtil.SoundEnum.MaleHurtShort);
            rip.Close();
        }

        public void UpdateCardControl(Card card)
        {
            InitCardControl(card.BattleCard);
            UpdateCardControl(card.BattleCard);
            IsDead = (card.Condition == VikingSaga.Code.Card.CardConditionEnum.Defeated);
        }        

        public void InitCardControl(BattleCard card)
        {
            Card = card;
            BackgroundCardBrush.ImageSource = WPFGUIUtil.GetImage("\\mobs\\" + card.ImageUrl).Source;
            CardName.Text = card.Name;
            CardDescription.Text = card.Description;
            ManaCost.Text = card.Mana.ToString();
        }

        public void UpdateCardControl(BattleCard card)
        {
            if (card == null)
                return;

            if (card is CardInstant)
            {
                var c = (CardInstant)card;
                HP.Text = "";
                Attack.Text = "";
            }
            else if (card is CardBasicMob)
            {
                var c = (CardBasicMob)card;
                UpdateHp(c);
                UpdateDmg(c);
            }
        }

        private void UpdateHp(CardBasicMob c)
        {
            var oldHp = HP.Text;
            HP.Text = c.Hp.ToString();

            SolidColorBrush brush = Brushes.White;
            if (c.Hp > c.HpBase)
                brush = Brushes.LightGreen;
            else if (c.Hp < c.HpBase)
                brush = Brushes.Yellow;

            HP.Foreground = brush;

            if (!string.IsNullOrWhiteSpace(oldHp) && oldHp != HP.Text)
            {
                AnimHelper.ApplyNumberChangeAnim(vbHp);
            }
        }

        private void UpdateDmg(CardBasicMob c)
        {
            var oldAttack = Attack.Text;
            Attack.Text = c.Dmg.ToString();

            SolidColorBrush brush = Brushes.White;
            if (c.Dmg > c.DmgBase)
                brush = Brushes.LightGreen;
            else if (c.Dmg < c.DmgBase)
                brush = Brushes.Red;

            Attack.Foreground = brush;

            if (!string.IsNullOrWhiteSpace(oldAttack) && oldAttack != Attack.Text)
            {
                AnimHelper.ApplyNumberChangeAnim(vbAttack);
            }
        }

        public bool CanAfford { get { return Card.Owner.Mana >= Card.Mana; } }

        public void BringToFront()
        {
            var parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            parent.Children.Add(this);
        }

        public void FadeKill()
        {
            var fadeAnim = AnimHelper.GetAnim(1, 0, 500);
            fadeAnim.Completed += (s, args) =>
            {
                Kill();
            };
            this.BeginAnimation(OpacityProperty, fadeAnim);
        }

        public void Kill()
        {
            var parent = (Grid)this.Parent;
            parent.Children.Remove(this);
            this.OwningPlaceholder.ClearCardControl();
            Visibility = System.Windows.Visibility.Collapsed;
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

        private void RemoveDeathOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                DeadOverlayImage.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        private void ShowDeathOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                DeadOverlayImage.Visibility = System.Windows.Visibility.Visible;
            });
        }
    }
}
