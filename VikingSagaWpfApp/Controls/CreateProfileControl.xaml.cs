using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for CreateProfileControl.xaml
    /// </summary>
    public partial class CreateProfileControl : UserControl
    {
        public delegate void StoreProfileCancelled();
        public event StoreProfileCancelled StoreProfileCancelledEvent;
        public void OnStoreProfileCancelled()
        {
            if (StoreProfileCancelledEvent != null)
                StoreProfileCancelledEvent();
        }

        public delegate void StoreProfileSucceess(VikingSagaUserProfile profile);
        public event StoreProfileSucceess StoreProfileSucceessEvent;
        public void OnStoreProfileSucceessEvent(VikingSagaUserProfile profile)
        {
            if (StoreProfileSucceessEvent != null)
                StoreProfileSucceessEvent(profile);
        }

        public CreateProfileControl()
        {
            InitializeComponent();
        }

        private void btnCreateNewProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileName = ProfileName.Text;
            var profilePW = Password.Password;
            var profileRepeatPW = RepeatedPassword.Password;

            if (profilePW.Length < 6)
            {
                StatusMessage.Text = "Password must be atleast 6 characters long!";
                return;
            }

            if (profilePW != profileRepeatPW)
            {
                StatusMessage.Text = "Passwords does not match!";
                return;
            }

            var restoredProfile = DAC.RestoreProfile(profileName);
            if (restoredProfile != null)
            {
                StatusMessage.Text = "Profile name is already taken!";
                return;
            }
                
            StoreProfile(profileName, profilePW);
        }

        // TODO Move to GameController
        private void StoreProfile(String profileName, String profilePW)
        {
            //var cardImageURL = @"heroes\nord-viking.jpg";
            var map = MapFactory.CreateMap(MapFactory.MAP1);
            //Hero hero = new Warrior { Name = profileName, HP = 10, Mana = 3, Level = 1, XP = 0, Gold = 0, CardImageURL = cardImageURL, Map = map };
            var profile = new VikingSagaUserProfile { Name = profileName, Password = profilePW, Gold = 0, Deck = CardFactory.CreateCampaignStarterDeck(), SelectedHero = null };

            DAC.StoreProfile(profile);
            var restoredProfile = DAC.RestoreProfile(profileName);

            OnStoreProfileSucceessEvent(profile);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OnStoreProfileCancelled();
        }
    }
}
