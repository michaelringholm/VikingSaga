using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Vik.Code.Controls.Base;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Utility
{
    public partial class LargeOverlayTextControl : UserControl
    {
        public LargeOverlayTextControl()
        {
            InitializeComponent();
        }

        private ControlAdorner _adorner;

        public void Display(string formattedText, int fadeInMs, int fadeOutMs, int stayMs, double topPosition)
        {
            UiUtil.SetTextBlockText(tbInfo, formattedText);

            _adorner = new ControlAdorner(VikGame.ScreenManager.MainWindow.MainGrid);
            _adorner.Child = this;

            double top = VikGame.ScreenManager.MainWindow.ActualHeight * topPosition;
            _adorner.Margin = new Thickness(0, top, 0, 0);
            _adorner.Width = VikGame.ScreenManager.MainWindow.ActualWidth;

            var layer = AdornerLayer.GetAdornerLayer(VikGame.ScreenManager.MainWindow.MainGrid);
            layer.Add(_adorner);

            SequentialActions.RunAsync(DoFading(fadeInMs, fadeOutMs, stayMs));
        }

        private IEnumerable<int> DoFading(int fadeInMs, int fadeOutMs, int stayMs)
        {
            AnimHelper.ApplyFadeAnimation(this, 0, 1, fadeInMs);
            yield return stayMs;
            AnimHelper.ApplyFadeAnimation(this, 1, 0, fadeOutMs);
            yield return fadeOutMs;

            var layer = AdornerLayer.GetAdornerLayer(VikGame.ScreenManager.MainWindow.MainGrid);
            layer.Remove(_adorner);
        }
    }
}
