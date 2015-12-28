using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vik.Code.Controls.Base;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Utility
{
    public partial class FloatingInfo : UserControl
    {
        // Display parameters
        public string Text = string.Empty;
        public new int FontSize = 20;
        public Color Color = Colors.Yellow;
        public double VerticalTravel = -50;
        public int StayMs = 2000;
        public int FadeInMs = 100;
        public int FadeOutMs = 500;
        public bool BounceIn = false;
        public int BounceMs = 500;
        public double BounceAmplitude = 2;

        private double StrokeWidth = 1;

        private Geometry _textGeometry;
        private SolidColorBrush _textBrush;
        private Pen _stroke;

        private double _calculatedWidth;
        private double _calculatedHeight;

        private ControlAdorner _adorner;

        private TranslateTransform _translateTrans = new TranslateTransform();
        private ScaleTransform _scaleTrans = new ScaleTransform();

        public FloatingInfo()
        {
            InitializeComponent();

            Info.Visibility = System.Windows.Visibility.Hidden;
            this.Opacity = 0.0;

            var tg = new TransformGroup();
            tg.Children.Add(_scaleTrans);
            tg.Children.Add(_translateTrans);
            this.RenderTransform = tg;
        }

        public void Display(double x, double y)
        {
            _textBrush = new SolidColorBrush(this.Color);
            _stroke = new System.Windows.Media.Pen(Brushes.Black, StrokeWidth);

            GenerateTextGeometry(this.Text);

            _scaleTrans.CenterX = _calculatedWidth / 2;
            _scaleTrans.CenterY = _calculatedHeight / 2;

            _translateTrans.X = x - _calculatedWidth / 2;
            _translateTrans.Y = y - _calculatedHeight / 2;

            _adorner = UiUtil.AddGlobalControlAdorner(this);

            SequentialActions.RunAsync(RunAsync());
        }

        private IEnumerable<int> RunAsync()
        {
            if (this.BounceIn)
            {
                var scaleAnim = new DoubleAnimation(0, 1, AnimHelper.Duration(this.BounceMs), FillBehavior.HoldEnd);
                var scaleEase = new BackEase();
                scaleEase.EasingMode = EasingMode.EaseOut;
                scaleEase.Amplitude = this.BounceAmplitude;
                scaleAnim.EasingFunction = scaleEase;
                _scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
                _scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            }

            double yTo = _translateTrans.Y + this.VerticalTravel;
            AnimHelper.ApplyAnimation(_translateTrans, TranslateTransform.YProperty, _translateTrans.Y, yTo, this.FadeInMs + this.StayMs + this.FadeOutMs);

            AnimHelper.ApplyFadeAnimation(this, 0, 1, this.FadeInMs);
            yield return this.FadeInMs + this.StayMs;

            AnimHelper.ApplyFadeAnimation(this, 1, 0, this.FadeOutMs);
            yield return this.FadeOutMs;

            UiUtil.RemoveGlobalControlAdorner(_adorner);
        }

        private void GenerateTextGeometry(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(
                    Info.FontFamily,
                    Info.FontStyle,
                    Info.FontWeight,
                    Info.FontStretch),
                    this.FontSize,
                    System.Windows.Media.Brushes.Black); // This brush does not matter since we use the geometry of the text. 

            _calculatedWidth = formattedText.Width;
            _calculatedHeight = formattedText.Height;

            // Build the geometry object that represents the text.
            _textGeometry = formattedText.BuildGeometry(new System.Windows.Point());
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Draw the outline based on the properties that are set.
            drawingContext.DrawGeometry(_textBrush, _stroke, _textGeometry);
        }
    }
}
