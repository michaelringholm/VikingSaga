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
using VikingSaga.Code.Resources;

namespace VikingSagaWpfApp.Windows
{
    /// <summary>
    /// Interaction logic for PreBattleWindow.xaml
    /// </summary>
    public partial class PostBattleWindow : Window, IPostBattleUI
    {
        public PostBattleWindow()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void btnAttack_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.EngageEncounter();
        }

        private void ShowCenteredDialog()
        {
            var mainWindow = Application.Current.MainWindow;
            Owner = mainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;

            ShowDialog();
        }

        public void ShowYouWonScreen(Encounter encounter)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var popup = new PostBattleWindow();
                popup.tbPostCombatText.Text = "You won and gained [" + encounter.Treasure.XP + "] XP and [" + encounter.Treasure.Gold + "] gold pieces!";
                popup.BackgroundImageBrush.ImageSource = ResourceManager.GetImage(ResourceManager.ImageEnum.BattleWonBackground).Source;
                SoundUtil.PauseMP3Loop();
                SoundUtil.PlaySound(SoundUtil.SoundEnum.BattleWon);
                SoundUtil.ResumeMP3Loop();
                popup.ShowCenteredDialog();
            }));
        }

        public void ShowYouLostScreen(Encounter encounter)
        {
            Dispatcher.Invoke(new Action(() =>
            {                
                //MessageBox.Show(Application.Current.MainWindow, );
                var popup = new PostBattleWindow();
                popup.tbPostCombatText.Text = "You were defeated and lost [" + encounter.Treasure.XP + "] XP.";
                popup.BackgroundImageBrush.ImageSource = ResourceManager.GetImage(ResourceManager.ImageEnum.BattleLostBackground).Source;
                SoundUtil.PauseMP3Loop();
                SoundUtil.PlaySound(SoundUtil.SoundEnum.BattleLost);
                SoundUtil.ResumeMP3Loop();
                popup.ShowCenteredDialog();
            }));
        }

        public void ShowLevelGained(Hero hero)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                //MessageBox.Show(Application.Current.MainWindow, );
                var popup = new PostBattleWindow();
                popup.tbPostCombatText.Text = "You gained a new level! Your experience in life has made you stronger and you are now level " + hero.Level + "!";
                popup.BackgroundImageBrush.ImageSource = ResourceManager.GetImage(ResourceManager.ImageEnum.BattleWonBackground).Source;
                SoundUtil.PauseMP3Loop();
                SoundUtil.PlaySound(SoundUtil.SoundEnum.LevelGained);
                SoundUtil.ResumeMP3Loop();
                popup.ShowCenteredDialog();
            }));
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
