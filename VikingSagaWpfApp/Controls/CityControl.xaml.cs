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
using VikingSagaWpfApp.Animations;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for CityControl.xaml
    /// </summary>
    public partial class CityControl : UserControl, ICityUI
    {
        public CityControl()
        {
            InitializeComponent();
            borderInfo.Opacity = 0;
        }

        public ImageSource GetMainWindowBackgroundImage()
        {
            return ResourceManager.GetImage(@"backgrounds/viking-village-1280x720.jpg").Source;
        }

        public void SetStatusMessage(string msg, int ms = 1500)
        {
            SequentialActions.RunAsync(InternalStatusMessage(msg, ms));
        }

        private IEnumerable<int> InternalStatusMessage(string msg, int ms = 1500)
        {
            tbInfo.Text = msg;
            AnimHelper.ApplyFadeAnimation(borderInfo, 0, 1, 200);
            yield return ms;
            AnimHelper.ApplyFadeAnimation(borderInfo, 1, 0, 300);
        }

        private void CardValkyrie_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //SetStatusMessage("All your cards has been revived!");
            //GameController.Current.ReviveCard(new Card());
            GameController.Current.ShowValkyrieGraveyard();
        }

        private void CardMerchant_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //SetStatusMessage("You have just bought a new rabbit!");
            //GameController.Current.BuyCard(new Card());
            GameController.Current.ShowMerchantShop();
        }

        private void CardSeer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameController.Current.ShowSeerHut();
        }

        private void CardSmith_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameController.Current.ShowSmithHouse();
        }

        private void CardBarmaid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameController.Current.TryEnterLonghouse();
        }


        public void Show(string message)
        {
            SoundUtil.PlaySound(SoundUtil.SoundEnum.ImportantMessage);
            SetStatusMessage(message, 5000);
        }
    }
}
