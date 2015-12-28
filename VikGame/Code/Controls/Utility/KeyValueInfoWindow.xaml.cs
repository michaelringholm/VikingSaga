using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vik.Code.Controls.Utility
{
    public partial class KeyValueInfoWindow : Window
    {
        public KeyValueInfoWindow()
        {
            InitializeComponent();

            GameLib.Utility.KeyValueDebugInfo.Items.CollectionChanged += Items_CollectionChanged;

            lvItems.ItemsSource = GameLib.Utility.KeyValueDebugInfo.Items;

            Closed += KeyValueInfoWindow_Closed;
        }

        void KeyValueInfoWindow_Closed(object sender, System.EventArgs e)
        {
            GameLib.Utility.KeyValueDebugInfo.Items.CollectionChanged -= Items_CollectionChanged;
        }

        private IColorTint _lastTint;

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var color = Color.FromArgb(150, 255, 255, 0);
            if (_lastTint != null)
                _lastTint.Remove();

            _lastTint = UiUtil.AddColorFlashingAdorner(this.lvItems, color, 3, 200, 200, 500);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
