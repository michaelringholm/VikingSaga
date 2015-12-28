using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Utility
{
    public interface IColorTint
    {
        void Remove();
    }

    internal class ColorAdorner : Adorner, IColorTint
    {
        private int _flashInMs;
        private int _flashOutMs;
        private int _pauseMs;

        private SolidColorBrush _brush;
        private Pen _pen;

        private AdornerLayer _layer;

        public ColorAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.IsHitTestVisible = false;
        }

        private int _flashCount = 0;
        private bool _stop = false;

        public void Display(Color color, double opacity)
        {
            this.Opacity = opacity;
            FlashThenDie(color, 0);
        }

        public void FlashThenDie(Color color, int flashCount, int flashInMs = 400, int flashOutMs = 1000, int pauseMs = 500)
        {
            VisualBrush vb = new VisualBrush(this.AdornedElement);
            vb.Stretch = Stretch.None;
            vb.AlignmentX = AlignmentX.Left;
            vb.AlignmentY = AlignmentY.Top;
            this.OpacityMask = vb;

            _flashCount = flashCount;
            _flashInMs = flashInMs;
            _flashOutMs = flashOutMs;
            _pauseMs = pauseMs;

            CreateBrush(color);

            _layer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            if (_layer == null)
                throw new InvalidOperationException("Adorner layer not created yet (usually created in OnLoad). Don't call this method from a constructor of a visual.");

            _layer.Add(this);

            if (_flashCount > 0)
            {
                this.Opacity = 0.0;
                SequentialActions.RunAsync(RunAsync());
            }
        }

        public void Remove()
        {
            if (_layer != null)
                _layer.Remove(this);

            _stop = true;
        }

        private void CreateBrush(Color color)
        {
            _brush = new SolidColorBrush(color);
            _pen = new Pen(_brush, 0);
        }

        private IEnumerable<int> RunAsync()
        {
            const double MaxOpacity = 0.9;

            for (int i = 0; i < _flashCount; ++i)
            {
                AnimHelper.ApplyFadeAnimation(this, 0.0, MaxOpacity, _flashInMs, SimpleEase.CubicOut);
                yield return _flashInMs;

                if (_stop)
                    break;

                AnimHelper.ApplyFadeAnimation(this, MaxOpacity, 0.0, _flashOutMs, SimpleEase.CubicOut);
                yield return _flashOutMs + _pauseMs;

                if (_stop)
                    break;
            }

            Remove();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect rec = new Rect(this.AdornedElement.RenderSize);
            drawingContext.DrawRectangle(_brush, _pen, rec);
        }
    }
}
