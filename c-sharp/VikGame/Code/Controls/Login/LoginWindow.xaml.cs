using System.Windows.Controls;
using System.Linq;
using System.Windows;
using Vik.Code.Utility;
using Vik.Code.Controls.Utility;
using System.Windows.Media;
using System.IO;
using System;
using System.Windows.Threading;

namespace Vik.Code.Controls.Login
{
    public partial class LoginWindow : FakeWindow
    {
        public string SelectedProfile { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            RemoveAllAdornersOnClose = true;
            Refresh();

            _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timer.Tick += _timer_Tick;
            _timer.Start();

            OnClosing += LoginWindow_OnClosing;
        }

        void LoginWindow_OnClosing(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            AddStar();
        }

        private Random _rnd = new Random(DateTime.Now.Millisecond);
        private DispatcherTimer _timer = new DispatcherTimer();

        private void AddStar()
        {
            double x = _rnd.NextDouble() * 0.9 + 0.05;
            double y = _rnd.NextDouble() * 0.7 + 0.05;
            int verticalTravel = _rnd.Next(50, 150);
            UiUtil.ShowFloatingInfo("*", VikGame.ScreenManager.XPos(x), VikGame.ScreenManager.YPos(y), true, Colors.White, false, 0, 0, verticalTravel, 30, 1000, 2000, 2000);
        }

        private void Refresh()
        {
            var profiles = VikGame.ProfileManager.ExistingProfiles().ToList();
            Profiles.ItemsSource = null;
            Profiles.Items.Clear();
            Profiles.ItemsSource = profiles;
        }

        private void AskDeleteProfile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return;

            var selection = UiUtil.VikMessageBox(
                string.Format("<F+><F+><B>Delete profile: {0}</B><F-><F-><BR><BR>Are you sure you want to <C ORANGE><B>PERMANENTLY</B><C DEFAULT> delete this profile?", profileName),
                VikMessageBoxButtons.YESNO);

            if (selection == Result.Yes)
            {
                selection = UiUtil.VikMessageBox(
                    "<F+><F+><B>Delete profile</B><F-><F-><BR><BR>Really, really sure?",
                    VikMessageBoxButtons.YESNO);

                if (selection == Result.Yes)
                {
                    if (!string.IsNullOrWhiteSpace(profileName))
                    {
                        VikGame.ProfileManager.DeleteProfile(profileName);

                        string saveFile = Util.GetStoreFileForProfile(profileName);
                        File.Delete(saveFile);

                        Refresh();

                        UiUtil.ShowFloatingInfo("Profile Deleted", 0.5, 0.3, true, Colors.Red, false, 0, 0, 0, 30, 1000, 1000, 3000);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string profileName = ((FrameworkElement)sender).Tag as string;
            AskDeleteProfile(profileName);
        }

        private void SelectProfile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return;

            SelectedProfile = profileName;
            VikGame.ProfileManager.MoveToTop(profileName);

            this.Close(Result.OK);
        }

        private void btnSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string profileName = ((FrameworkElement)sender).Tag as string;
            SelectProfile(profileName);
        }

        private void btnCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var enterNameWindow = new EnterProfileNameWindow();
            var result = VikGame.ScreenManager.ShowContentModal(enterNameWindow, enterNameWindow.tbName, true);
            if (result == Result.OK)
            {
                string newProfileName = enterNameWindow.tbName.Text;
                if (string.IsNullOrWhiteSpace(newProfileName))
                    return;

                var existingProfiles = VikGame.ProfileManager.ExistingProfiles().Select(p => p.ToLowerInvariant());
                if (existingProfiles.Contains(newProfileName.ToLowerInvariant()))
                {
                    Util.ErrorPopup("Error", "Name {0} already exists", newProfileName);
                    return;
                }

                // Make sure no old save files are picked up
                string saveFile = Util.GetStoreFileForProfile(newProfileName);
                File.Delete(saveFile);

                VikGame.ProfileManager.CreateProfile(newProfileName);
                SelectedProfile = newProfileName;

                this.Close(Result.OK);
            }
        }
    }
}
