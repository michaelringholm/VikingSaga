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
    /// Interaction logic for ProfileHomeControl.xaml
    /// </summary>
    public partial class ProfileHomeControl : UserControl, IProfileUI
    {
        public ProfileHomeControl()
        {
            InitializeComponent();
            //Background = ResourceManager.GetImageBrush("backgrounds\\general-form-background-1900x1200.jpg");
            //InitHeroDetails(GameController.Current.Profile.Hero);
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                Background = Brushes.Transparent;
        }

        public void Show(VikingSagaUserProfile profile)
        {
            Visibility = Visibility.Visible;
            UpdateProfileDetails(profile);
        }

        public void UpdateProfileDetails(VikingSagaUserProfile profile)
        {
            heroCardControl1.UpdateHeroCard(profile.SelectedHero);
            heroCardControl2.UpdateHeroCard(new Druid { CardImageURL = @"heroes/druid-hero.png" });
            heroCardControl3.UpdateHeroCard(new Sorcerer { CardImageURL = @"heroes/sorcerer-hero.png" });
            heroCardControl4.UpdateHeroCard(new Hunter { CardImageURL = @"heroes/hunter-hero.png" });
        }

        private void heroCardControl2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameController.Current.ShowCreateHero();
        }

        public ImageSource GetMainWindowBackgroundImage()
        {
            return ResourceManager.GetImage(@"backgrounds/battle-form-background-1900x1200.jpg").Source;
        }
    }
}
