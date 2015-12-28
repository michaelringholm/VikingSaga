using GameLib.Battles.Cards;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vik.Code.Controls.Cards;
using Vik.Code.Utility;
using VikingSaga.Code;

namespace Vik.Code.Controls.Utility
{
    class BattleCardHelper
    {
        private class Param
        {
            public double Rotation { get; set; }
            public Point Position { get; set; }
        }

        private bool _zoomMode;
        private CardControl _control;
        private Param _cardOriginParam;
        private Param _dstParam;
        private bool _isAligning;

        private RotateTransform _rotateTransform = new RotateTransform();
        private ScaleTransform _scaleTransform = new ScaleTransform();
        private TranslateTransform _translateTransform = new TranslateTransform();
        private TransformGroup _transGroup = new TransformGroup();

        public BattleCardHelper()
        {
            _transGroup.Children.Add(_scaleTransform);
            _transGroup.Children.Add(_rotateTransform);
            _transGroup.Children.Add(_translateTransform);
        }

        public void Init(CardControl control)
        {
            _control = control;
            _cardOriginParam = GetParam(_control);
            _control.RenderTransform = _transGroup;

            AttachEvents(_control);
        }

        public bool IsAligning { get { return _isAligning; } }

        Param GetParam(FrameworkElement element)
        {
            var result = new Param();
            result.Position = new Point();

            if (element.RenderTransform is RotateTransform)
            {
                result.Rotation = ((RotateTransform)element.RenderTransform).Angle;
            }
            else if (element.RenderTransform is TransformGroup)
            {
                foreach (Transform trans in ((TransformGroup)element.RenderTransform).Children)
                {
                    if (trans is RotateTransform)
                    {
                        result.Rotation = ((RotateTransform)trans).Angle;
                        break;
                    }
                }
            }

            result.Rotation = result.Rotation % 360;
            return result;
        }

        Duration Duration(int ms)
        {
            return new Duration(new TimeSpan(0, 0, 0, 0, ms));
        }

        public void SetTranslateXY(double x, double y)
        {
            _translateTransform.X = x;
            _translateTransform.Y = y;
        }

        public void AlignToElement(FrameworkElement element, int ms)
        {
            _isAligning = true;

            _dstParam = GetParam(element);

            // Adjust so alignment to larger controls will center in the middle (but limit to smallest axis, for very wide or tall controls)
            double centerX = (element.ActualWidth - _control.ActualWidth) / 2;
            double centerY = (element.ActualHeight - _control.ActualHeight) / 2;

            // Adjust for very wide or very tall destinations. In these cases skip the longest axis.
            if (element.ActualWidth > element.ActualHeight * 2)
            {
                centerX = _translateTransform.X - _control.ActualWidth;
            }

            _dstParam.Position = element.TranslatePoint(new Point(centerX, centerY), VikGame.ScreenManager.MainWindow);

            Param srcParam = GetParam(_control);

            var easing = new QuinticEase();

            var xAnim = new DoubleAnimation(_translateTransform.X, _dstParam.Position.X, Duration(ms), FillBehavior.Stop);
            var yAnim = new DoubleAnimation(_translateTransform.Y, _dstParam.Position.Y, Duration(ms), FillBehavior.Stop);
            var rotAnim = new DoubleAnimation(srcParam.Rotation, _dstParam.Rotation, Duration(ms), FillBehavior.Stop);
            rotAnim.Completed += rotAnim_Completed;

            xAnim.EasingFunction = easing;
            yAnim.EasingFunction = easing;
            rotAnim.EasingFunction = easing;

            _rotateTransform.CenterX = _control.ActualWidth / 2;
            _rotateTransform.CenterY = _control.ActualHeight / 2;

            _translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnim);
            _translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnim);
            _rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotAnim);
        }

        void rotAnim_Completed(object sender, EventArgs e)
        {
            _isAligning = false;
        }

        private void DetachEvents(CardControl control)
        {
            control.MouseLeftButtonDown -= control_MouseLeftButtonDown;
            control.MouseLeave -= control_MouseLeave;
        }

        private void AttachEvents(CardControl control)
        {
            control.MouseLeftButtonDown += control_MouseLeftButtonDown;
            control.MouseLeave += control_MouseLeave;
        }

        void control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseClick();
        }

        const double ZoomMs = 100;

        void DoZoom()
        {
            if (_zoomMode)
                return;

            if (_control.Card != null && _control.OwningPlaceholder != null)
            {
                var placeHolder = _control.OwningPlaceholder;
                _scaleTransform.CenterX = placeHolder.ActualWidth / 2;
                _scaleTransform.CenterY = placeHolder.ActualHeight / 2;
            }

            var timeScale = (2.0 - _scaleTransform.ScaleX);
            var animZoom = AnimHelper.GetAnim(_scaleTransform.ScaleX, 2.0, (int)(ZoomMs * timeScale));
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animZoom);
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animZoom);

            _control.BringToFront();
            _zoomMode = true;
        }

        void EndZoom()
        {
            if (!_zoomMode)
                return;

            var timeScale = 1.0 - (2.0 - _scaleTransform.ScaleX);
            var animZoom = AnimHelper.GetAnim(_scaleTransform.ScaleX, 1.0, (int)(ZoomMs * timeScale));
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animZoom);
            _scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animZoom);

            _zoomMode = false;
        }

        void control_MouseLeave(object sender, MouseEventArgs e)
        {
            _control.ReleaseMouseCapture();
            EndZoom();
        }

        void OnMouseClick()
        {
            if (_zoomMode)
                EndZoom();
            else if (_control.IsZoomable)
                DoZoom();
        }
    }
}
