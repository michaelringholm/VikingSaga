using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VikingSaga.Code;
using VikingSagaWpfApp.Animations;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSagaWpfApp
{
    class DragHelper
    {
        private class Param
        {
            public double Rotation { get; set; }
            public Thickness Margin { get; set; }
        }

        private bool _zoomMode;
        private bool _mouseClickPending;
        private CardControl _control;
        private Param _dstParam;
        private Param _beginDragParam;
        private Point _beginDragPoint;
        private CardPlaceholder _highlightedPlaceholder;
        private HeroCardControl _highlightedHero;
        private bool _isDragging;
        private bool _isAligning;

        private RotateTransform _rotateTransform = new RotateTransform();
        private ScaleTransform _scaleTransform = new ScaleTransform();
        private TranslateTransform _translateTransform = new TranslateTransform();
        private TransformGroup _transGroup = new TransformGroup();

        public void Init(CardControl control)
        {
            _control = control;

            _transGroup.Children.Add(_scaleTransform);
            _transGroup.Children.Add(_rotateTransform);
            _transGroup.Children.Add(_translateTransform);
            _control.RenderTransform = _transGroup;

            AttachEvents(_control);
        }

        public bool IsAligning { get { return _isAligning; } }

        Param GetParam(UserControl c)
        {
            var result = new Param();
            result.Margin = c.Margin;

            if (c.RenderTransform is RotateTransform)
            {
                result.Rotation = ((RotateTransform)c.RenderTransform).Angle;
            }
            else if (c.RenderTransform is TransformGroup)
            {
                foreach (Transform trans in ((TransformGroup)c.RenderTransform).Children)
                {
                    if (trans is RotateTransform)
                    {
                        result.Rotation = ((RotateTransform)trans).Angle;
                        break;
                    }
                }
            }

            return result;
        }

        Duration Duration(int ms)
        {
            return new Duration(new TimeSpan(0, 0, 0, 0, ms));
        }

        public void AlignToUserControl(UserControl control, int ms)
        {
            _isAligning = true;

            _dstParam = GetParam(control);
            // Adjust so alignment to larger controls will center in the middle
            double centerX = (control.ActualWidth - _control.Width) / 2;
            double centerY = (control.ActualHeight - _control.Height) / 2;
            _dstParam.Margin = new Thickness(_dstParam.Margin.Left + centerX, _dstParam.Margin.Top + centerY, 0, 0);

            Param srcParam = GetParam(_control);

            var easing = new QuinticEase();

            var xAnim = new DoubleAnimation(srcParam.Margin.Left, _dstParam.Margin.Left, Duration(ms), FillBehavior.Stop);
            var yAnim = new DoubleAnimation(srcParam.Margin.Top, _dstParam.Margin.Top, Duration(ms), FillBehavior.Stop);
            var rotAnim = new DoubleAnimation(srcParam.Rotation, _dstParam.Rotation, Duration(ms), FillBehavior.Stop);
            rotAnim.Completed += rotAnim_Completed;

            xAnim.EasingFunction = easing;
            yAnim.EasingFunction = easing;
            rotAnim.EasingFunction = easing;

            _rotateTransform.CenterX = _control.Width / 2;
            _rotateTransform.CenterY = _control.Height / 2;

            _translateTransform.BeginAnimation(TranslateTransform.XProperty, xAnim);
            _translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnim);
            _rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotAnim);

            _control.Margin = new Thickness();
        }

        void rotAnim_Completed(object sender, EventArgs e)
        {
            _control.Margin = _dstParam.Margin;
            _translateTransform.X = 0;
            _translateTransform.Y = 0;
            _rotateTransform.Angle = _dstParam.Rotation;
            _isAligning = false;
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
                _scaleTransform.CenterX = placeHolder.Width / 2;
                _scaleTransform.CenterY = placeHolder.Height / 2;

                if (placeHolder.IsHand)
                {
                    var battle = GameEngine.Current.CurrentBattle;
                    if (battle != null)
                    {
                        var yOffset = battle.IsPlayer1(_control.Card.Owner) ? placeHolder.Height * 0.5 : -placeHolder.Height * 0.5;
                        _scaleTransform.CenterY += yOffset;
                    }
                }
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

            if (!_control.IsMouseOver)
                return;

            if (_mouseClickPending)
            {
                OnMouseClick(e);
                _mouseClickPending = false;
                return;
            }

            if (!_isDragging || e.LeftButton == MouseButtonState.Pressed)
                return;

            _isDragging = false;
            _control.Margin = new Thickness(_translateTransform.X, _translateTransform.Y, 0, 0);
            _translateTransform.X = 0;
            _translateTransform.Y = 0;

            if (_highlightedPlaceholder != null)
            {
                _highlightedPlaceholder.ShowReadyForDrop(false);
                _highlightedPlaceholder = null;
            }

            if (_highlightedHero != null)
            {
                _highlightedHero.ShowReadyForDrop(false);
                _highlightedHero = null;
            }

            Rect rect = new Rect(_control.Margin.Left, _control.Margin.Top, _control.ActualWidth, _control.ActualHeight);

            var hero = GameController.Current.BattleBoardUI.GetOverlappedHero(rect);
            if (hero != null && hero.CanReceiveDroppedCard(_control))
            {
                // Dropped on hero
                hero.DropCardOnHero(_control);
            }
            else
            {
                var placeholder = GameController.Current.BattleBoardUI.GetOverlappedPlaceholder(rect);
                if (placeholder != null && placeholder.CanReceiveDroppedCard(_control))
                {
                    if (_control.Card is CardInstant)
                    {
                        placeholder.DropCardOnOtherCard(_control);
                    }
                    else
                    {
                        placeholder.DropOnEmptyBoardPlaceHolder(_control);
                        AlignToUserControl(placeholder, 200);
                    }
                }
                else
                {
                    AlignToUserControl(_control.OwningPlaceholder, 500);
                }
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

            var mousePos = e.GetPosition((IInputElement)_control.Parent);
            var deltaX = _beginDragParam.Margin.Left + (mousePos.X - _beginDragPoint.X);
            var deltaY = _beginDragParam.Margin.Top + (mousePos.Y - _beginDragPoint.Y);
            _translateTransform.X = deltaX;
            _translateTransform.Y = deltaY;

            Rect rect = new Rect(_translateTransform.X, _translateTransform.Y, _control.ActualWidth, _control.ActualHeight);

            var hero = GameController.Current.BattleBoardUI.GetOverlappedHero(rect);
            if (_highlightedHero != null && _highlightedHero != hero)
            {
                _highlightedHero.ShowReadyForDrop(false);
                _highlightedHero = null;
            }

            if (hero != null && hero.CanReceiveDroppedCard(_control))
            {
                // Mouse over hero
                _highlightedHero = hero;
                _highlightedHero.ShowReadyForDrop(true);
            }

            var placeholder = GameController.Current.BattleBoardUI.GetOverlappedPlaceholder(rect);
            if (_highlightedPlaceholder != null && _highlightedPlaceholder != placeholder)
            {
                _highlightedPlaceholder.ShowReadyForDrop(false);
                _highlightedPlaceholder = null;
            }

            if (placeholder != null)
            {
                // Mouse over placholder
                if (placeholder.CanReceiveDroppedCard(_control))
                {
                    _highlightedPlaceholder = placeholder;
                    _highlightedPlaceholder.ShowReadyForDrop(true);
                }
            }
        }

        void TryBeginDrag(MouseEventArgs e)
        {
            if (!_control.CanAfford)
            {
                _control.Card.AddUiOutput("Not enough mana");
                GameController.Current.BattleBoardUI.ShowNotifications();
                _mouseClickPending = false;
                return;
            }

            _isDragging = true;
            _mouseClickPending = false;

            _beginDragParam = GetParam(_control);
            _beginDragPoint = e.GetPosition((IInputElement)_control.Parent);

            _translateTransform.X = _beginDragParam.Margin.Left;
            _translateTransform.Y = _beginDragParam.Margin.Top;

            _control.Margin = new Thickness();

            var rotAnim = new DoubleAnimation(_beginDragParam.Rotation, 0, Duration(100), FillBehavior.HoldEnd);
            _rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotAnim);

            _control.RenderTransform = _transGroup;
            _control.BringToFront();
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
