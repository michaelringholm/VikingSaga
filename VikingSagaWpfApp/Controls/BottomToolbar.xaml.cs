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

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for HomeToolbar.xaml
    /// </summary>
    public partial class BottomToolbar : UserControl
    {
        public BottomToolbar()
        {
            InitializeComponent();
        }

        private void btnCampaign_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.ShowMap();
        }

        private void btnVillagePit_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.StartRandomBattle();
        }

        private void btnEditDeck_Click(object sender, RoutedEventArgs e)
        {
            new EditDeckWindow(GameController.Current.Profile).Show();
        }

        private void btnVisitVillage_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.TryEnterVillage();
        }

        private void ButtonControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameController.Current.ShowQuestList();
        }
    }
}
