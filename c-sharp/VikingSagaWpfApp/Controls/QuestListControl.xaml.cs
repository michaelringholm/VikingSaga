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
using VikingSaga.Code.Campaign;
using VikingSaga.Code.Resources;
using VikingSaga.Controls;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ProfileHomeControl.xaml
    /// </summary>
    public partial class QuestListControl : UserControl, IQuestListUI
    {
        private int _deckStartIndex = 0;
        private int _maxDeckSize = 0;
        private Deck _deck;
        private Hero _hero;

        public QuestListControl()
        {
            InitializeComponent();
        }

        public void Show(List<QuestProgress> quests)
        {
            Visibility = Visibility.Visible;
            lvQuests.Items.Clear();
            foreach(var questProgress in quests)
                lvQuests.Items.Add(new QuestItemControl { QuestTitle = questProgress.Title, Status = "In progress" });
        }

        private void Update(Hero hero, Deck deck)
        {
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
