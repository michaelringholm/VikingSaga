using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Vik.Code.Controls.Base
{
    /// <summary>
    /// Interaction logic for TabControl.xaml
    /// </summary>
    public partial class TabControl : UserControl
    {
        [Description("The tab title."), Category("Viking")]
        public String Title 
        { 
            get{ return TabTitle.Text; } 
            set{ TabTitle.Text = value; } 
        }

        private bool _selected;
        [Description("Whether or not the tab is selected."), Category("Viking")]
        public bool Selected 
        {
            get { return _selected; }
            set 
            { 
                _selected = value;
                if(_selected)
                {
                    OuterBorder.Background = (Brush)FindResource("OrangeGradientBrush");
                    TabTitle.FontWeight = FontWeights.Bold;
                }
                else
                {
                    OuterBorder.Background = (Brush)FindResource("GreyGradientBrush");
                    TabTitle.FontWeight = FontWeights.Normal;
                }
            }
        }

        [Description("Thrown when a tab is selected."), Category("Viking")] 
        public event EventHandler TabSelected;

        private void SelectTab(object sender, MouseButtonEventArgs e)
        {            
            this.Selected = true;

            if (TabSelected != null)
                TabSelected(this, EventArgs.Empty);
        }

        public TabControl()
        {
            InitializeComponent();
        }
    }
}
