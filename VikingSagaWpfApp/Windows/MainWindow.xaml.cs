using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using VikingSaga.Code;
using VikingSagaWpfApp.Code;

namespace VikingSagaWpfApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowUI
    {
        public MainWindow()
        {
            InitializeComponent();

            //SoundUtil.PlayMP3Loop(@"music/medieval-fantasy1.mp3");
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = 0; // (screenWidth / 2) - (windowWidth / 2);
            this.Top = 0; // (screenHeight / 2) - (windowHeight / 2);

            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
        }

        public void ChangeBodyContent(IUIControl uiControl)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    BodyContent.Content = (System.Windows.Controls.UserControl)uiControl;
                    BackgroundImageBrush.ImageSource = uiControl.GetMainWindowBackgroundImage();
                }
                catch(Exception)
                {
                    BodyContent.Content = null;
                }
            }));
        }


        public void DisableNonCombatButtons()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TopToolbar.Visibility = System.Windows.Visibility.Hidden;
                BottomToolbar.Visibility = System.Windows.Visibility.Hidden;
            }));
        }

        public void EnableNonCombatButtons()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TopToolbar.Visibility = System.Windows.Visibility.Visible;
                BottomToolbar.Visibility = System.Windows.Visibility.Visible;
            }));
        }
    }
}
