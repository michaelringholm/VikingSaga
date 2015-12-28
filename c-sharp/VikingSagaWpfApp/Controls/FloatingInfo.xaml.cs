using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VikingSagaWpfApp.Animations;

namespace VikingSagaWpfApp.Controls
{
    public class FloatingInfoParam
    {
        public enum CategoryType { SpellInfo, HeroInfo, HpChange, ManaChange, DmgChange };

        public static FloatingInfoParam Create(Control control, string text, Color color, CategoryType category)
        {
            var result = new FloatingInfoParam();
            result.Control = control;
            result.Text = text;
            result.Color = color;
            return result;
        }

        public static FloatingInfoParam Create(Control control, string text, Color color, CategoryType category, double scrollAmount = 20, int scrollTimeMs = 3000, bool bounceIn = false)
        {
            var result = Create(control, text, color, category);
            result.ScrollAmount = scrollAmount;
            result.ScrollTimeMs = scrollTimeMs;
            result.BounceIn = bounceIn;
            return result;
        }

        public CategoryType Category;
        public Control Control;
        public string Text = string.Empty;
        public int FontSize = 28;
        public Color Color = Colors.White;
        public Point Offset = new Point();
        public double ScrollAmount = -30;
        public int ScrollTimeMs = 3000;
        public bool BounceIn = true;
    }

    public partial class FloatingInfo : UserControl
    {
        const double FixedWidth = 150;
        const double FixedHeight = 25;

        private Geometry _textGeometry;
        private Geometry _textHighLightGeometry;
        private SolidColorBrush _textBrush;
        private Pen _stroke;

        private bool Highlight = false;
        private double StrokeWidth = 1;

        public FloatingInfo()
        {
            InitializeComponent();

            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Visibility = System.Windows.Visibility.Collapsed;
            Info.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Show(double x, double y, FloatingInfoParam param)
        {
            Info.Text = param.Text;

            x -= FixedWidth / 2;
            y -= FixedHeight / 2;

            _textBrush = new SolidColorBrush(param.Color);
            _stroke = new System.Windows.Media.Pen(Brushes.Black, StrokeWidth);
            SetText(param.Text, param);

            ApplyAnimations(x, y, param);
            Visibility = System.Windows.Visibility.Visible;
        }

        void anim_Completed(object sender, System.EventArgs e)
        {
            KillMe();
        }

        private void ApplyAnimations(double x, double y, FloatingInfoParam param)
        {
            var transTrans = new TranslateTransform();
            var scaleTrans = new ScaleTransform();
            scaleTrans.CenterX = FixedWidth / 2;
            scaleTrans.CenterY = FixedHeight / 2;

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(scaleTrans);
            tg.Children.Add(transTrans);
            RenderTransform = tg;

            int msFadeOut = 500;
            int msMid = param.ScrollTimeMs;
            int msBounce = 500;
            int totalMs = msMid + msFadeOut;

            var floatAnim = new DoubleAnimation(y, y + param.ScrollAmount, AnimHelper.Duration(totalMs), FillBehavior.HoldEnd);
            transTrans.X = x;
            transTrans.Y = y;
            transTrans.BeginAnimation(TranslateTransform.YProperty, floatAnim);

            var fadeOutAnim = new DoubleAnimation(1, 0, AnimHelper.Duration(msFadeOut), FillBehavior.HoldEnd);
            fadeOutAnim.BeginTime = AnimHelper.TimeSpan(msMid);
            this.BeginAnimation(UIElement.OpacityProperty, fadeOutAnim);

            if (param.BounceIn)
            {
                var scaleAnim = new DoubleAnimation(0, 1, AnimHelper.Duration(msBounce), FillBehavior.HoldEnd);
                var scaleEase = new BackEase();
                scaleEase.EasingMode = EasingMode.EaseOut;
                scaleEase.Amplitude = 2;
                scaleAnim.EasingFunction = scaleEase;
                scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
                scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            }

            floatAnim.Completed += anim_Completed;
        }

        private void KillMe()
        {
            ((Panel)this.Parent).Children.Remove(this);
        }

        public void SetText(string text, FloatingInfoParam param)
        {
            // Create the formatted text based on the properties set.
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(
                    Info.FontFamily,
                    Info.FontStyle,
                    Info.FontWeight,
                    Info.FontStretch),
                    param.FontSize,
                    System.Windows.Media.Brushes.Black); // This brush does not matter since we use the geometry of the text. 

            // Build the geometry object that represents the text.
            _textGeometry = formattedText.BuildGeometry(new System.Windows.Point(FixedWidth / 2 - formattedText.Width / 2, 0));

            // Build the geometry object that represents the text hightlight. 
            if (Highlight == true)
            {
                _textHighLightGeometry = formattedText.BuildHighlightGeometry(new System.Windows.Point(0, 0));
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Draw the outline based on the properties that are set.
            drawingContext.DrawGeometry(_textBrush, _stroke, _textGeometry);

            // Draw the text highlight based on the properties that are set. 
            if (Highlight == true)
            {
                drawingContext.DrawGeometry(null, _stroke, _textHighLightGeometry);
            }
        }
    }
}
