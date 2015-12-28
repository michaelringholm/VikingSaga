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
    /// Interaction logic for TopToolbar.xaml
    /// </summary>
    public partial class TopToolbar : UserControl
    {
        private bool _isMuted = true;
        public TopToolbar()
        {
            InitializeComponent();
        }

        private void btnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.ShowProfile();
        }

        private void btnExitGame_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.ExitGame();
        }

        private void btnViewSelectedHero_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.ShowSelectedHero();
        }

        private void btnToggleMusic_Click(object sender, RoutedEventArgs e)
        {
            if (_isMuted)
            {
                SoundUtil.StartMP3Loop(@"music/medieval-fantasy1.mp3");
                _isMuted = false;
                MusicToggleImageBrush.ImageSource = ResourceManager.GetImage("buttons/mute-2-48.png").Source;
            }
            else
            {
                SoundUtil.StopCurrentMP3Loop();
                _isMuted = true;
                MusicToggleImageBrush.ImageSource = ResourceManager.GetImage("buttons/speaker-48.png").Source;
            }
        }
    }
}
