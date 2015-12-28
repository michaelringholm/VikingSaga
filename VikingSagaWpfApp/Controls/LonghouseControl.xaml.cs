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
using System.Windows.Media.Animation;
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
    public partial class LonghouseControl : UserControl, ILonghouseUI
    {
        private int _deckStartIndex = 0;
        private int _maxDeckSize = 0;
        private Deck _deck;
        private Hero _hero;

        public LonghouseControl()
        {
            InitializeComponent();
        }

        public void Show(Hero hero, Deck deck)
        {
            Show(hero, deck, null, false);
        }

        public void Show(Hero hero, Deck deck, String message, bool presentNewQuest)
        {
            Visibility = Visibility.Visible;
            Update(hero, deck);

            if (!String.IsNullOrEmpty(message))
            {
                //MessageInfo.BeginAnimation(new DependencyProperty(), new Animations.)
                SoundUtil.PlaySound(SoundUtil.SoundEnum.ImportantMessage);
                MessageInfo.InfoMessage = message;
            }

            if(presentNewQuest)
            {
                //TODO Right now we assume the barmaid always has the quest
                animate(CardBarmaid);
            }
        }

        private void animate(Control control)
        {
            var fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(5)
            };

            Storyboard.SetTarget(fade, control);
            Storyboard.SetTargetProperty(fade, new PropertyPath(Button.OpacityProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);

            sb.Begin();
        }

        private void Update(Hero hero, Deck deck)
        {
            heroCardControl.UpdateHeroCard(hero);

            heroCardControl.ToolTip = hero.Name;

            _deck = deck;
            _hero = hero;
            _maxDeckSize = deck.AllCards.Count;
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
            return ResourceManager.GetImage(@"backgrounds/longhouse-background.png").Source;
        }
    }
}
