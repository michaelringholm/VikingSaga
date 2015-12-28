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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Utility
{
    public enum VikMessageBoxButtons { OK, OKCANCEL, YESNO };

    public partial class VikMessageBoxWindow : FakeWindow
    {
        public VikMessageBoxWindow()
        {
            InitializeComponent();
            ButtonPanel.Children.Clear();

            Opacity = 0.0;
            Loaded += delegate { AnimHelper.ApplyPopInAnimation(this); };
        }

        public UIElement AddButton(Result result)
        {
            var newButton = new Button();
            newButton.Content = ResultAsDisplayString(result);
            newButton.Margin = new Thickness(20, 0, 20, 0);
            newButton.Visibility = System.Windows.Visibility.Visible;
            newButton.Tag = result;
            newButton.Click += newButton_Click;
            ButtonPanel.Children.Add(newButton);

            return newButton;
        }

        void newButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var result = (Result)button.Tag;
            Close(result);
        }
    }
}
