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

namespace VikingSagaWpfApp.Windows
{
    /// <summary>
    /// Interaction logic for SurrenderWindow.xaml
    /// </summary>
    public partial class SurrenderWindow : Window, ISurrenderUI
    {
        public SurrenderWindow()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void btnSurrender_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.SurrenderNow();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.CancelSurrender();
        }

        public new void Show()
        {
            //EnemyHeroCardControl.UpdateHeroCard(encounter.Hero);
            tbSurrenderText.Text = "Do you really wanna surrender? Most likely your encounter will kill you anyways and you will loose hard earned XP from dying!";

            var mainWindow = Application.Current.MainWindow;
            Owner = mainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            ShowDialog();
        }
    }
}
