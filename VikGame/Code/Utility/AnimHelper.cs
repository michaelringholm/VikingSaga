﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Vik.Code.Utility
{
    public enum SimpleEase { None, CubicOut };

    public static class AnimHelper
    {
        public static PathGeometry PathFromPoints(IEnumerable<Point> points)
        {
            var list = points.ToList();
            if (list.Count < 2)
                return new PathGeometry();

            PathGeometry result = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            pFigure.StartPoint = list[0];

            var segment = new PolyLineSegment();
            for (int i = 0; i < list.Count; ++i)
            {
                segment.Points.Add(list[i]);
            }

            pFigure.Segments.Add(segment);
            result.Figures.Add(pFigure);

            result.Freeze();
            return result;
        }

        public static Duration Duration(int ms)
        {
            return new Duration(TimeSpan(ms));
        }

        public static TimeSpan TimeSpan(int ms)
        {
            return new TimeSpan(0, 0, 0, 0, ms);
        }

        public static DoubleAnimationUsingPath GetPathAnim(PathGeometry path, int ms, PathAnimationSource source)
        {
            var result = new DoubleAnimationUsingPath();
            result.PathGeometry = path;
            result.Duration = AnimHelper.Duration(ms);
            result.Source = source;
            return result;
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

        public static DoubleAnimation ApplyAnimation(IAnimatable animatable, DependencyProperty prop, double from, double to, int ms, SimpleEase ease = SimpleEase.None, int startTime = 0)
        {
            var anim = GetAnim(from, to, ms, startTime);
            if (ease != SimpleEase.None)
            {
                var easingFunc = new CubicEase();
                easingFunc.EasingMode = EasingMode.EaseOut;
                anim.EasingFunction = easingFunc;
            }

            animatable.BeginAnimation(prop, anim);
            return anim;
        }

        public static DoubleAnimation ApplyFadeAnimation(IAnimatable animatable, double from, double to, int ms, SimpleEase ease = SimpleEase.None, int startTime = 0)
        {
            return ApplyAnimation(animatable, UIElement.OpacityProperty, from, to, ms, ease, startTime);
        }

        public static void ApplyPopInAnimation(FrameworkElement element)
        {
            ApplyFadeAnimation(element, 0.0, 1.0, 300, SimpleEase.CubicOut);
            ApplyScaleAnimation(element, 0.5, 1.0, 200, SimpleEase.CubicOut);
        }

        public static void ApplyScaleAnimation(FrameworkElement element, double from, double to, int ms, SimpleEase ease = SimpleEase.None, double centerX = 0.5, double centerY = 0.5)
        {
            var scaleTrans = new ScaleTransform();
            scaleTrans.CenterX = element.ActualWidth * centerX;
            scaleTrans.CenterY = element.ActualHeight * centerY;
            element.RenderTransform = scaleTrans;

            AnimHelper.ApplyAnimation(scaleTrans, ScaleTransform.ScaleXProperty, 0.5, 1.0, ms, SimpleEase.CubicOut);
            AnimHelper.ApplyAnimation(scaleTrans, ScaleTransform.ScaleYProperty, 0.5, 1.0, ms, SimpleEase.CubicOut);
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

        //static public void ApplyKillCardAnimation(CardControl control, int ms, Action postAction = null)
        //{
        //    DoubleAnimation opacityAnim = new DoubleAnimation(1, 0, Duration(ms), FillBehavior.Stop);
        //    if (postAction != null)
        //        opacityAnim.Completed += (s, e) => { postAction(); };

        //    control.BeginAnimation(CardControl.OpacityProperty, opacityAnim);

        //    DoubleAnimation scaleAnim = new DoubleAnimation(1.0, 0.8, Duration(ms), FillBehavior.Stop);
        //    //scaleAnim.EasingFunction = new CubicEase();
        //    ScaleTransform trans = new ScaleTransform();
        //    trans.CenterX = control.Width / 2;
        //    trans.CenterY = control.Height / 2;
        //    control.RenderTransform = trans;
        //    trans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
        //    trans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        //}

        //static public void ApplyCardVsCardAnimation(Control src, Control dst, int ms)
        //{
        //    var transEase = new CubicEase();
        //    transEase.EasingMode = EasingMode.EaseIn;
        //    DoubleAnimation scaleAnim = new DoubleAnimation(1.0, 1.2, Duration(ms / 2), FillBehavior.Stop);
        //    scaleAnim.EasingFunction = new CubicEase();
        //    scaleAnim.AutoReverse = true;

        //    double offsetX = (dst.ActualWidth - src.ActualWidth) / 2;
        //    double offsetY = (dst.ActualHeight - src.ActualHeight) / 2;
        //    double distX = (dst.Margin.Left - src.Margin.Left) + offsetX;
        //    double distY = (dst.Margin.Top - src.Margin.Top) + offsetY;

        //    ScaleTransform scaleTrans = new ScaleTransform();
        //    scaleTrans.CenterX = src.Width / 2;
        //    scaleTrans.CenterY = src.Height / 2;
        //    scaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
        //    scaleTrans.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

        //    DoubleAnimation transAnimY = new DoubleAnimation(0, distY, Duration(ms), FillBehavior.Stop);
        //    transAnimY.AutoReverse = true;
        //    transAnimY.EasingFunction = transEase;

        //    DoubleAnimation transAnimX = new DoubleAnimation(0, distX, Duration(ms), FillBehavior.Stop);
        //    transAnimX.AutoReverse = true;
        //    transAnimX.EasingFunction = transEase;

        //    TranslateTransform transTrans = new TranslateTransform();
        //    transTrans.BeginAnimation(TranslateTransform.YProperty, transAnimY);
        //    transTrans.BeginAnimation(TranslateTransform.XProperty, transAnimX);

        //    TransformGroup tg = new TransformGroup();
        //    tg.Children.Add(transTrans);
        //    tg.Children.Add(scaleTrans);

        //    src.RenderTransform = tg;
        //}
    }
}
