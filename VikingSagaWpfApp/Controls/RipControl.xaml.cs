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
using VikingSagaWpfApp.Animations;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for RipControl.xaml
    /// </summary>
    public partial class RipControl : UserControl
    {
        public RipControl()
        {
            InitializeComponent();
            Opacity = 1;
        }

        private Grid _parent;

        public void Run(Grid parent)
        {
            Visibility = System.Windows.Visibility.Visible;
            _parent = parent;
            Grid.SetRowSpan(this, 999);
            Grid.SetColumnSpan(this, 999);
            DoubleAnimation scaleAnim = new DoubleAnimation(2, 1.0, AnimHelper.Duration(100), FillBehavior.HoldEnd);
            ScaleTransform trans = new ScaleTransform();
            trans.CenterX = parent.ActualWidth / 2;
            trans.CenterY = parent.ActualHeight / 2;
            RenderTransform = trans;
            trans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            trans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            AnimHelper.ApplyFadeAnimation(this, 0, 1, 10);
        }

        public void Close()
        {
            var fadeOut = AnimHelper.GetAnim(1, 0, 500);
            fadeOut.Completed += (s2, e2) =>
            {
                _parent.Children.Remove(this);
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
    }
}
