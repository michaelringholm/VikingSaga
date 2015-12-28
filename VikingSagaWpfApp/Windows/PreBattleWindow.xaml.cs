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
using System.Windows.Shapes;
using VikingSaga.Code;

namespace VikingSagaWpfApp.Windows
{
    /// <summary>
    /// Interaction logic for PreBattleWindow.xaml
    /// </summary>
    public partial class PreBattleWindow : Window, IPreBattleUI
    {
        public PreBattleWindow()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.EngageEncounter();
        }

        public void Show(Encounter encounter)
        {
            EnemyHeroCardControl.UpdateHeroCard(encounter.Hero);
            tbPreCombatText.Text = encounter.PreCombatText;

            var mainWindow = Application.Current.MainWindow;
            Owner = mainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
            SoundUtil.PlaySound(SoundUtil.SoundEnum.Danger);

            ShowDialog();
        }

        private void btnFlee_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.FleeFromEncounter();
        }
    }
}
