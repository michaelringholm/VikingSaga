using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VikingSagaWpfApp.Animations
{
    public static class AnimHelper
    {
        public static Duration Duration(int ms)
        {
            return new Duration(TimeSpan(ms));
        }

        public static TimeSpan TimeSpan(int ms)
        {
            return new TimeSpan(0, 0, 0, 0, ms);
        }

        public static DoubleAnimation GetAnim(double from, double to, int ms, int startTime = 0)
        {
            DoubleAnimation result = new DoubleAnimation(from, to, Duration(ms), FillBehavior.HoldEnd);
            if (startTime != 0)
            {
                result.BeginTime = TimeSpan(startTime);
            }
            return result;
        }

        public static void ApplyFadeAnimation(UIElement control, double from, double to, int ms, int startTime = 0)
        {
            var anim = GetAnim(from, to, ms, startTime);
            control.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        public static void ApplyNumberChangeAnim(FrameworkElement element)
        {
            const int ms = 500;
            var scaleAnim = new DoubleAnimation(1, 2, AnimHelper.Duration(ms), FillBehavior.Stop);
            var scaleEase = new BackEase();
            scaleEase.EasingMode = EasingMode.EaseOut;
            scaleEase.Amplitude = 1;
            scaleAnim.EasingFunction = scaleEase;
            scaleAnim.AutoReverse = true;

            var scaleTrans = new ScaleTransform();
            scaleTrans.CenterX = element.ActualWidth / 2;
            scaleTrans.CenterY = element.ActualHeight / 2;
            scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            element.RenderTransform = scaleTrans;
        }

        static public void ApplyHeroDamageAnimation(HeroCardControl control)
        {
            //DoubleAnimation scaleAnim = new DoubleAnimation(1, 0.9, Duration(200), FillBehavior.Stop);
            //scaleAnim.EasingFunction = new CubicEase();

            //ScaleTransform scaleTrans = new ScaleTransform();
            //scaleTrans.CenterX = 180 / 2;
            //scaleTrans.CenterY = 212 / 2;
            //scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            //scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            //control.RenderTransform = scaleTrans;
        }

        static public void ApplyCardDamageAnimation(CardControl control)
        {
            //DoubleAnimation scaleAnim = new DoubleAnimation(1, 0.9, Duration(600), FillBehavior.Stop);
            //scaleAnim.EasingFunction = new CubicEase();

            //ScaleTransform scaleTrans = new ScaleTransform();
            //scaleTrans.CenterX = control.Width / 2;
            //scaleTrans.CenterY = control.Height / 2;
            //scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            //scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            //control.RenderTransform = scaleTrans;
        }

        static public void ApplyKillCardAnimation(CardControl control, int ms, Action postAction = null)
        {
            DoubleAnimation opacityAnim = new DoubleAnimation(1, 0, Duration(ms), FillBehavior.Stop);
            if (postAction != null)
                opacityAnim.Completed += (s, e) => { postAction(); };

            control.BeginAnimation(CardControl.OpacityProperty, opacityAnim);

            DoubleAnimation scaleAnim = new DoubleAnimation(1.0, 0.8, Duration(ms), FillBehavior.Stop);
            //scaleAnim.EasingFunction = new CubicEase();
            ScaleTransform trans = new ScaleTransform();
            trans.CenterX = control.Width / 2;
            trans.CenterY = control.Height / 2;
            control.RenderTransform = trans;
            trans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            trans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        static public void ApplyCardVsCardAnimation(Control src, Control dst, int ms)
        {
            var transEase = new CubicEase();
            transEase.EasingMode = EasingMode.EaseIn;
            DoubleAnimation scaleAnim = new DoubleAnimation(1.0, 1.2, Duration(ms / 2), FillBehavior.Stop);
            scaleAnim.EasingFunction = new CubicEase();
            scaleAnim.AutoReverse = true;

            double offsetX = (dst.ActualWidth - src.ActualWidth) / 2;
            double offsetY = (dst.ActualHeight - src.ActualHeight) / 2;
            double distX = (dst.Margin.Left - src.Margin.Left) + offsetX;
            double distY = (dst.Margin.Top - src.Margin.Top) + offsetY;

            ScaleTransform scaleTrans = new ScaleTransform();
            scaleTrans.CenterX = src.Width / 2;
            scaleTrans.CenterY = src.Height / 2;
            scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            DoubleAnimation transAnimY = new DoubleAnimation(0, distY, Duration(ms), FillBehavior.Stop);
            transAnimY.AutoReverse = true;
            transAnimY.EasingFunction = transEase;

            DoubleAnimation transAnimX = new DoubleAnimation(0, distX, Duration(ms), FillBehavior.Stop);
            transAnimX.AutoReverse = true;
            transAnimX.EasingFunction = transEase;

            TranslateTransform transTrans = new TranslateTransform();
            transTrans.BeginAnimation(TranslateTransform.YProperty, transAnimY);
            transTrans.BeginAnimation(TranslateTransform.XProperty, transAnimX);

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(transTrans);
            tg.Children.Add(scaleTrans);

            src.RenderTransform = tg;
        }
    }
}
