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
    /// Interaction logic for CreateHeroControl.xaml
    /// </summary>
    public partial class CreateHeroControl : UserControl, ICreateHeroUI
    {
        public CreateHeroControl()
        {
            InitializeComponent();

            PaintCards();
        }

        public void Update(VikingSagaUserProfile Profile)
        {
            
        }

        private void PaintCards()
        {
            HeroCard1.BackgroundCardBrush.ImageSource = ResourceManager.GetImage("heroes/warrior-hero.png").Source;
            HeroCard2.BackgroundCardBrush.ImageSource = ResourceManager.GetImage("heroes/druid-hero.png").Source;
            HeroCard3.BackgroundCardBrush.ImageSource = ResourceManager.GetImage("heroes/sorcerer-hero.png").Source;
            HeroCard4.BackgroundCardBrush.ImageSource = ResourceManager.GetImage("heroes/hunter-hero.png").Source;
        }

        private void btnCreateNewHero_Click(object sender, RoutedEventArgs e)
        {
            int heroIndexClicked = 0;
            GameController.Current.CreateNewHero(HeroName.Text, typeof(Warrior), heroIndexClicked);
        }

        public ImageSource GetMainWindowBackgroundImage()
        {
            return ResourceManager.GetImage(@"backgrounds/battle-form-background-1900x1200.jpg").Source;
        }
    }
}
