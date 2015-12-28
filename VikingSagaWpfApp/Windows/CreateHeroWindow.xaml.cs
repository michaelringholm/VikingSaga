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
    /// Interaction logic for CreateHeroWindow.xaml
    /// </summary>
    public partial class CreateHeroWindow : Window
    {
        public CreateHeroWindow()
        {
            InitializeComponent();
        }

        private void btnCreateNewHero_Click(object sender, RoutedEventArgs e)
        {
            GameController.Current.CreateHero(HeroName.Text, SelectedHeroClass);
            //GameEngine.GetMap(GameEngine.MapEnum.DefaultWorld);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedRadioButton = (RadioButton)sender;
            SelectedHeroClass = selectedRadioButton.CommandParameter.ToString();
        }

        public string SelectedHeroClass { get; set; }
    }
}
