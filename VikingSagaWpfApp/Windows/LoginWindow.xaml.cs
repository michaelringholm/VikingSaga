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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VikingSaga.Code;
using VikingSagaWpfApp.Controls;
using VikingSagaWpfApp.Windows;
using VikingSaga.Windows;
using VikingSaga.Code.Resources;

namespace VikingSagaWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            //Log.Init(SynchronizationContext.Current);

            try
            {
                ResourceManager.CheckResources();
                CardEngineStatic.Init(SynchronizationContext.Current);

                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                double windowWidth = this.Width;
                double windowHeight = this.Height;
                this.Left = (screenWidth / 2) - (windowWidth / 2);
                this.Top = (screenHeight / 2) - (windowHeight / 2);
                Background = ResourceManager.GetImageBrush("backgrounds/viking-village-1280x720.jpg");

                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
            }            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var profile = DAC.RestoreProfile(tbProfileName.Text);
                if (profile != null && profile.Password == tbPassword.Password)
                {
                    GameEngine.StartGameEngine(profile); // TODO only engine or controller should hold profile
                    GameController.Current.Profile = profile;

                    var mainWindow = new MainWindow();
                    GameController.Current.MainWindowUI = mainWindow;
                    mainWindow.ShowInTaskbar = false;
                    mainWindow.Owner = null; // Application.Current.MainWindow;
                    //mainWindow.BodyContent.Content = new ProfileHomeControl();
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();

                    GameController.Current.ShowProfile();
                    this.Close();
                }
                else
                {
                    var brush = new SolidColorBrush();
                    brush.Color = Color.FromRgb(255, 127, 42);
                    tbProfileName.BorderBrush = brush;
                    tbProfileName.BorderThickness = new Thickness(2);
                    tbProfileName.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblCreateNewProfile_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new CreateProfileWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateTestProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Creating test profile");
                DAC.CreateTestProfile();
                MessageBox.Show("Creating test profile done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
