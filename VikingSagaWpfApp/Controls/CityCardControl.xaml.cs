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

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for CityCardControl.xaml
    /// </summary>
    public partial class CityCardControl : UserControl
    {
        public String Title
        {
            get { return tbTitle.Text; }
            set { tbTitle.Text = value; }
        }

        public String Description
        {
            get { return tbDescription.Text; }
            set { tbDescription.Text = value; }
        }

        public ImageSource CardImageSource
        {
            get { return imgCard.Source; }
            set { imgCard.Source = value; }
        }

        public CityCardControl()
        {
            InitializeComponent();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            MainGrid.Opacity = 1.0;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            MainGrid.Opacity = 0.7;
        }
    }
}
