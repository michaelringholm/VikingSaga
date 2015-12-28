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

namespace VikingSagaWpfApp
{
    /// <summary>
    /// Interaction logic for CreateProfileWindow.xaml
    /// </summary>
    public partial class CreateProfileWindow : Window
    {
        public CreateProfileWindow()
        {
            InitializeComponent();
            CreateProfileControl.StoreProfileCancelledEvent += CreateProfileControl_StoreProfileCancelledEvent;
            CreateProfileControl.StoreProfileSucceessEvent += CreateProfileControl_StoreProfileSucceessEvent;
        }

        void CreateProfileControl_StoreProfileSucceessEvent(VikingSagaUserProfile profile)
        {
            GameController.Current.Profile = profile;
            GameController.Current.ShowProfile();
            this.Close();
        }

        void CreateProfileControl_StoreProfileCancelledEvent()
        {
            this.Close();
        }
    }
}
