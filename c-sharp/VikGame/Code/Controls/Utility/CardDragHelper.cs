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
    class CardDragHelper
    {
        private class Param
        {
            public double Rotation { get; set; }
            public Point Position { get; set; }
        }

        public FrameworkElement DragDropParent;

        private bool _zoomMode;
        private bool _mouseClickPending;
        private CardControl _control;
        private Param _dstParam;
        private Param _beginDragParam;
        private Point _beginDragPoint;
        private bool _isDragging;
        private bool _isAligning;

        private ControlAdorner _adorner;
        private CardControl _adornerCardControl;
        private AdornerLayer _adornerLayer;

        private RotateTransform _rotateTransform = new RotateTransform();
        private ScaleTransform _scaleTransform = new ScaleTransform();
        private TranslateTransform _translateTransform = new TranslateTransform();
        private TransformGroup _transGroup = new TransformGroup();

        public CardDragHelper()
        {
            _transGroup.Children.Add(_scaleTransform);
            _transGroup.Children.Add(_rotateTransform);
            _transGroup.Children.Add(_translateTransform);
        }

        public void Init(CardControl control)
        {
            _control = control;
            AttachEvents(_control);
        }

        public bool IsAligning { get { return _isAligning; } }

        Param GetParam(FrameworkElement element)
        {
            var result = new Param();
            result.Position = element.TranslatePoint(new Point(), VikGame.ScreenManager.MainWindow);

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

        private void ApplyFadeOut()
        {
            AnimHelper.ApplyFadeAnimation(_adorner, 1.0, 0.0, 500, SimpleEase.CubicOut);
        }

        void rotAnim_Completed(object sender, EventArgs e)
        {
            _isAligning = false;
            _adorner.Visibility = Visibility.Hidden;
            _control.Opacity = 1.0;
        }

        private void DetachEvents(CardControl control)
        {
            control.MouseDown -= control_MouseDown;
            control.MouseMove -= control_MouseMove;
            control.MouseUp -= control_MouseUp;
            control.MouseLeave -= control_MouseLeave;
        }

        private void AttachEvents(CardControl control)
        {
            control.MouseDown += control_MouseDown;
            control.MouseMove += control_MouseMove;
            control.MouseUp += control_MouseUp;
            control.MouseLeave += control_MouseLeave;
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
            _control.RenderTransform = _transGroup;
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

        void OnMouseClick(MouseEventArgs e)
        {
            if (_zoomMode)
                EndZoom();
            else if (_control.IsZoomable)
                DoZoom();
        }

        void control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _control.ReleaseMouseCapture();

            if (_mouseClickPending)
            {
                OnMouseClick(e);
                _mouseClickPending = false;
                return;
            }

            if (!_isDragging || e.LeftButton == MouseButtonState.Pressed)
                return;

            _isDragging = false;

            var args = SendCardDragEvent(CardDragEventType.DropQueryAccept);
            SendDroppedEvent(args);

            if (args.Accept)
            {
                if (args.AcceptingElement != null)
                {
                    // Align to the control the card was dropped on
                    AlignToElement(args.AcceptingElement, 500);
                }

                ApplyFadeOut();
            }
            else
            {
                // Move back to where we came from
                AlignToElement(_control.OwningPlaceholder, 500);
            }
        }

        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_control.IsDraggable || _isAligning || e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_mouseClickPending)
                TryBeginDrag(e);

            if (!_isDragging)
                return;

            var mousePos = e.GetPosition(VikGame.ScreenManager.MainWindow);
            var deltaX = mousePos.X - _beginDragPoint.X;
            var deltaY = mousePos.Y - _beginDragPoint.Y;
            _translateTransform.X = deltaX;
            _translateTransform.Y = deltaY;

            Rect rect = new Rect(_translateTransform.X, _translateTransform.Y, _control.ActualWidth, _control.ActualHeight);
            SendCardDragEvent(CardDragEventType.DragMove);
        }

        private void EnsureAdorner()
        {
            if (_adorner == null)
            {
                // TODO: If card control is scaled (like in a viewbox) actual width and height will still be unscaled,
                // and thus the adorner will not match in size
                _adorner = new ControlAdorner(VikGame.ScreenManager.MainWindow.MainGrid);
                _adornerCardControl = new CardControl(_control.Card, _control.DisplayFlags);
                _adornerCardControl.Width = _control.ActualWidth;
                _adornerCardControl.Height = _control.ActualHeight;

                _adorner.Child = _adornerCardControl;

                _adornerLayer = AdornerLayer.GetAdornerLayer(VikGame.ScreenManager.MainWindow.MainGrid);
                _adornerLayer.Add(_adorner);
            }

            _adorner.Visibility = Visibility.Visible;
            _control.Opacity = 0.0;
        }

        void TryBeginDrag(MouseEventArgs e)
        {
            EnsureAdorner();

            _isDragging = true;
            _mouseClickPending = false;

            _beginDragParam = GetParam(_control);
            _beginDragPoint = e.GetPosition(_control);

            _translateTransform.X = _beginDragParam.Position.X;
            _translateTransform.Y = _beginDragParam.Position.Y;

            var rotAnim = new DoubleAnimation(_beginDragParam.Rotation, 0, Duration(100), FillBehavior.HoldEnd);
            _rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotAnim);

            _adornerCardControl.RenderTransform = _transGroup;
            SendCardDragEvent(CardDragEventType.DragBegin);
        }

        private void SendDroppedEvent(CardDragEventArgs acceptedArgs)
        {
            acceptedArgs.DragEvent = CardDragEventType.Dropped;
            VikGame.ScreenManager.DragDropManager.OnCardDrag(DragDropParent, acceptedArgs);
        }

        private CardDragEventArgs SendCardDragEvent(CardDragEventType eventType)
        {
            var rect = new Rect(_translateTransform.X, _translateTransform.Y, _control.ActualWidth, _control.ActualHeight);
            var args = new CardDragEventArgs
            {
                Card = this._control.Card,
                DragEvent = eventType,
                DragRect = rect,
                OwningPlaceholder = _control.OwningPlaceholder
            };

            VikGame.ScreenManager.DragDropManager.OnCardDrag(DragDropParent, args);
            return args;
        }

        void control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(_control.IsDraggable || _control.IsZoomable))
                return;

            _control.CaptureMouse();
            if (_zoomMode)
            {
                OnMouseClick(e);
            }
            else
            {
                _mouseClickPending = true;
            }
        }
    }
}
