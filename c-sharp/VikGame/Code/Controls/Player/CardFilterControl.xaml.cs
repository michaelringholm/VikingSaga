using GameLib.Battles.Cards;
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

namespace Vik.Code.Controls.Player
{
    /// <summary>
    /// Interaction logic for CardFilterControl.xaml
    /// </summary>
    public partial class CardFilterControl : UserControl
    {        
        private CardBattle.CardFlagsEnum _cardFlags = CardBattle.CardFlagsEnum.All;
        [Description("The card filter type (enum)."), Category("Viking")]
        public CardBattle.CardFlagsEnum CardFlag 
        {
            get { return _cardFlags; }
            set 
            { 
                _cardFlags = value;
                SetMetaData(_cardFlags);
            }
        }

        private void SetMetaData(CardBattle.CardFlagsEnum cardFlags)
        {
            ImageSource imgSource;
            String imgPath;
            switch(cardFlags)
            {
                case CardBattle.CardFlagsEnum.Warrior:imgPath = "Data/Gfx/Buttons/classes/warrior.jpg"; FilterName.Text = "Warrior"; break;
                case CardBattle.CardFlagsEnum.Mage: imgPath = "Data/Gfx/Buttons/classes/mage.jpg"; FilterName.Text = "Mage"; break;
                case CardBattle.CardFlagsEnum.Priest: imgPath = "Data/Gfx/Buttons/classes/priest.jpg"; FilterName.Text = "Priest"; break;
                case CardBattle.CardFlagsEnum.Druid: imgPath = "Data/Gfx/Buttons/classes/druid.jpg"; FilterName.Text = "Druid"; break;
                case CardBattle.CardFlagsEnum.Hunter: imgPath = "Data/Gfx/Buttons/classes/hunter.jpg"; FilterName.Text = "Hunter"; break;
                case CardBattle.CardFlagsEnum.Minion: imgPath = "Data/Gfx/Buttons/classes/minion.jpg"; FilterName.Text = "Minion"; break;
                case CardBattle.CardFlagsEnum.Buff: imgPath = "Data/Gfx/Buttons/abilities/buff.png"; FilterName.Text = "Buff"; break;
                case CardBattle.CardFlagsEnum.DeBuff: imgPath = "Data/Gfx/Buttons/abilities/debuff.png"; FilterName.Text = "DeBuff"; break;
                case CardBattle.CardFlagsEnum.DD: imgPath = "Data/Gfx/Buttons/abilities/dd.png"; FilterName.Text = "DD"; break;
                case CardBattle.CardFlagsEnum.DoT: imgPath = "Data/Gfx/Buttons/abilities/dot.png"; FilterName.Text = "DoT"; break;
                case CardBattle.CardFlagsEnum.Heal: imgPath = "Data/Gfx/Buttons/abilities/heal.png"; FilterName.Text = "Heal"; break;
                default: imgPath = "Data/Gfx/Buttons/classes/warrior.jpg"; FilterName.Text = "Warrior"; break;
            }

            if (DesignerProperties.GetIsInDesignMode(this))
                imgSource = null; //new BitmapImage(new Uri("pack://application:,,,/" + imgPath.ToLower()));
            else
                imgSource = VikGame.ResourceLocator.GetImageResource(imgPath);
            FilterImage.Source = imgSource;
            
        }

        [Description("Whether or not the filter is selected/enabled."), Category("Viking")]
        private bool _selected;
        public bool Selected 
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (_selected)
                    this.Border.BorderBrush = (Brush)FindResource("ToggleButtonColorBrush");
                else
                    this.Border.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        [Description("Thrown when a filter is toggled/clicked."), Category("Viking")] 
        public event EventHandler FilterToggled;

        public CardFilterControl()
        {
            InitializeComponent();
        }

        private void ToggleFilter(object sender, MouseButtonEventArgs e)
        {
            ToggleButton();
            
            if (FilterToggled != null)
                FilterToggled(this, EventArgs.Empty);
        }

        public void ToggleButton()
        {
            this.Selected = !this.Selected;            
        }
    }
}
