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
using VikingSaga.Code;
using VikingSaga.Code.Resources;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ProfileHomeControl.xaml
    /// </summary>
    public partial class EditDeckControl : UserControl, IDeckEditUI
    {
        private int _deckStartIndex = 0;
        private int _maxDeckSize = 0;
        private Deck _deck;
        private Hero _hero;

        public EditDeckControl()
        {
            InitializeComponent();
            btnScrollLeft.Visibility = Visibility.Hidden;
            btnScrollRight.Visibility = Visibility.Hidden;
        }

        public void Show(Hero hero, Deck deck)
        {
            Visibility = Visibility.Visible;
            Update(hero, deck);
        }

        public void Update(Hero hero, Deck deck)
        {
            heroCardControl.UpdateHeroCard(hero);

            heroCardControl.ToolTip = hero.Name;
            tbGold.Text = hero.Gold.ToString();
            tbLevel.Text = hero.Level.ToString();

            pbXP.ToolTip = hero.XP.ToString() + "/" + hero.GetLevel().EndXP.ToString();
            pbXP.Minimum = hero.GetLevel().StartXP;
            pbXP.Maximum = hero.GetLevel().EndXP;
            pbXP.Value = hero.XP;

            PaintDeck(deck);

            btnScrollLeft.Visibility = Visibility.Visible;
            btnScrollRight.Visibility = Visibility.Visible;
            _deck = deck;
            _hero = hero;
            _maxDeckSize = deck.AllCards.Count;
        }

        private void PaintDeck(Deck deck)
        {
            Card[] cards = deck.AllCards.Skip(_deckStartIndex).Take(5).ToArray();

            VisibleCard1.UpdateCardControl(cards[0]);            
            VisibleCard2.UpdateCardControl(cards[1]);
            VisibleCard3.UpdateCardControl(cards[2]);
            VisibleCard4.UpdateCardControl(cards[3]);
            VisibleCard5.UpdateCardControl(cards[4]);
        }

        private void btnScrollLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_deckStartIndex > 0)
            {
                _deckStartIndex--;
                Update(_hero, _deck);
            }
        }

        private void btnScrollRight_Click(object sender, RoutedEventArgs e)
        {
            if (_deckStartIndex < _maxDeckSize-5)
            {
                _deckStartIndex++;
                Update(_hero, _deck);
            }
        }

        public ImageSource GetMainWindowBackgroundImage()
        {
            return ResourceManager.GetImage(@"backgrounds/hero-home-background3.png").Source;
        }
    }
}
