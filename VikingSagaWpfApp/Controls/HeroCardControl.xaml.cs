using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using VikingSaga.Code;
using VikingSaga.Code.Resources;
using VikingSagaWpfApp.Animations;
using VikingSagaWpfApp.Code.BattleNs;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp
{
    /// <summary>
    /// Interaction logic for HeroCardControl.xaml
    /// </summary>
    public partial class HeroCardControl : UserControl
    {
        public Player Player { get; set; }
        public bool IsPlayer1 { get; set; }

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

        public HeroCardControl()
        {
            InitializeComponent();
            IsSelected = false;
            HP.Text = string.Empty;
        }

        public void UpdateHeroCard(Hero hero)
        {
            Dispatcher.Invoke(() =>
            {
                if (hero != null)
                {
                    HP.Text = hero.RemainingHP.ToString();
                    HeroName.Text = hero.Name;
                    Mana.Text = hero.RemainingMana.ToString();
                    BackgroundCardBrush.ImageSource = ResourceManager.GetImage(hero.CardImageURL).Source;
                }
                else
                    BackgroundCardBrush.ImageSource = ResourceManager.GetImage(ResourceManager.ImageEnum.UnknownHeroCard).Source;
            });
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

        public IEnumerable<int> HeroAttackedEffectActions()
        {
            var rip = EffectHelper.ApplyRipEffect(this.MainGrid);
            SoundUtil.PlaySound("sounds/paper-rip.wav");

            var oldHp = HP.Text;
            HP.Text = Player.Hp.ToString();
            if (!string.IsNullOrWhiteSpace(oldHp) && oldHp != HP.Text)
            {
                AnimHelper.ApplyNumberChangeAnim(HP);
            }

            yield return 500;
            SoundUtil.PlaySound("short-hurt-male.wav");
            rip.Close();
        }

        private void ResetImage(object orgImageSrc)
        {
            Dispatcher.Invoke(() =>
            {
                BackgroundCardBrush.ImageSource = (ImageSource)orgImageSrc;
                //ExtensionMethods.Refresh(BackgroundCardBrush.ImageSource);
            });
        }

        public void DropCardOnHero(CardControl cardControl)
        {
            var card = cardControl.Card;
            if (card is CardInstant)
            {
                var player = card.Owner;
                Task.Run(() =>
                {
                    // This must not be called on UI thread, see SequentialActions.RunBlocking
                    player.DropCardOnPlayer(Player, (CardInstant)card, false);
                });
            }
        }

        public void ShowReadyForDrop(bool show)
        {
            Border.BorderBrush = show ? Brushes.Green : null;
        }

        public bool CanReceiveDroppedCard(CardControl cardControl)
        {
            BattleCard bc = cardControl.Card;

            bool targetOwnPlayerOk = bc.CanTargetOwnPlayer && IsPlayer1;
            bool targetEnemyPlayerOk = bc.CanTargetEnemyPlayer && !IsPlayer1;

            bool result = targetOwnPlayerOk || targetEnemyPlayerOk;
            return result;
        }
    }
}
