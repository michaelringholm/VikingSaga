using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Vik.Code.Controls.Base;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Utility
{
    public static class UiUtil
    {
        public static SolidColorBrush DragDropBorderHighlightBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        public static SolidColorBrush DragDropBorderDenyBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static SolidColorBrush DragDropBackgroundHighlightBrush = new SolidColorBrush(Color.FromArgb(80, 0, 100, 0));

        public static void SetDebugInfo(string s, params object[] args)
        {
            if (!VikGame.IsDeveloperMode)
                return;

            VikGame.ScreenManager.MainWindow.tbDebug.Text = string.Format(s, args);
        }

        public static readonly double CardW = 360;
        public static readonly double CardH = 230;
        public static readonly double CardAspectRatio = CardW / CardH;

        public static IColorTint AddColorTintAdorner(UIElement adornedElement, Color color, double opacity)
        {
            var colorAdorner = new ColorAdorner(adornedElement);
            colorAdorner.Display(color, opacity);
            return colorAdorner;
        }

        public static IColorTint AddColorFlashingAdorner(UIElement adornedElement, Color color, int flashCount, int flashInMs = 400, int flashOutMs = 1000, int pauseMs = 500)
        {
            var colorAdorner = new ColorAdorner(adornedElement);
            colorAdorner.FlashThenDie(color, flashCount, flashInMs, flashOutMs, pauseMs);
            return colorAdorner;
        }

        public static ControlAdorner AddGlobalControlAdorner(FrameworkElement child)
        {
            var controlAdorner = new ControlAdorner(VikGame.ScreenManager.MainWindow.MainGrid);
            controlAdorner.Child = child;

            var layer = VikGame.ScreenManager.GetGlobalAdornerLayer();
            layer.Add(controlAdorner);

            return controlAdorner;
        }

        public static void RemoveGlobalControlAdorner(ControlAdorner controlAdorner)
        {
            var layer = VikGame.ScreenManager.GetGlobalAdornerLayer();
            layer.Remove(controlAdorner);
        }

        public static void RemoveAllGlobalControlAdorners()
        {
            var layer = VikGame.ScreenManager.GetGlobalAdornerLayer();
            var allAdorners = layer.GetAdorners(VikGame.ScreenManager.MainWindow.MainGrid);
            if (allAdorners == null)
                return;

            foreach (var adorner in allAdorners)
            {
                layer.Remove(adorner);
            }
        }

        public static Rect GetWindowSpaceRect(FrameworkElement element)
        {
            Rect rectangleBounds = new Rect();
            Point topLeft = element.TransformToVisual(VikGame.ScreenManager.MainWindow).Transform(new Point(0, 0));
            rectangleBounds = element.RenderTransform.TransformBounds(new Rect(topLeft, element.RenderSize));
            return rectangleBounds;
        }

        public static void ShowFloatingInfo(
            string text,
            double x,
            double y,
            bool relativeCoordinates,
            Color color,
            bool bounceIn = false,
            int bounceMs = 500,
            double bounceAmplitude = 2.0,
            int verticalTravel = -50,
            int fontSize = 20,
            int fadeInMs = 100,
            int stayMs = 2000,
            int fadeOutMs = 500)
        {
            var info = new FloatingInfo();
            info.Text = text;
            info.Color = color;
            info.BounceIn = bounceIn;
            info.BounceMs = bounceMs;
            info.BounceAmplitude = bounceAmplitude;
            info.VerticalTravel = verticalTravel;
            info.FontSize = fontSize;
            info.FadeInMs = fadeInMs;
            info.StayMs = stayMs;
            info.FadeOutMs = fadeOutMs;

            double xx = relativeCoordinates ? VikGame.ScreenManager.XPos(x) : x;
            double yy = relativeCoordinates ? VikGame.ScreenManager.YPos(y) : y;
            info.Display(xx, yy);
        }

        public static void ShowLargeOverlayText(string formattedText, int fadeInMs = 1000, int fadeOutMs = 2000, int stayMs = 1500, double topPosition = 0.3)
        {
            var control = new LargeOverlayTextControl();
            control.Display(formattedText, fadeInMs, stayMs, fadeOutMs, topPosition);
        }

        public static void Init(SynchronizationContext uiContext)
        {
            _ctx = uiContext;
            UiManagedThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static int UiManagedThreadId;

        internal static SynchronizationContext _ctx;
        public static SynchronizationContext Ctx
        {
            get
            {
                if (_ctx == null)
                    throw new InvalidOperationException("Static init has not been called");
                return _ctx;
            }
        }

        public static void Send(Action action)
        {
            Ctx.Send((x) => action(), null);
        }

        public static void Post(Action action)
        {
            Ctx.Post((x) => action(), null);
        }

        public static void SetTextBlockText(TextBlock textBlock, string formattedText)
        {
            SetTextBlockText(textBlock, formattedText, Colors.White);
        }

        public static void SetTextBlockText(TextBlock textBlock, string formattedText, Color defaultColor)
        {
            textBlock.Inlines.Clear();
            var inlines = FormattedTextParser.Parse(formattedText, textBlock.FontFamily, textBlock.FontSize, defaultColor);
            textBlock.Inlines.AddRange(inlines);
        }

        // TODO: Shows behind map window when called from MapControl?
        public static FakeWindow.Result VikMessageBox(string formattedText, VikMessageBoxButtons buttons = VikMessageBoxButtons.OK, double width = double.NaN)
        {
            var win = new VikMessageBoxWindow();
            UIElement focusButton = null;

            SetTextBlockText(win.tbText, formattedText);

            if (buttons == VikMessageBoxButtons.OK)
            {
                focusButton = win.AddButton(FakeWindow.Result.OK);
            }
            else if (buttons == VikMessageBoxButtons.OKCANCEL)
            {
                focusButton = win.AddButton(FakeWindow.Result.OK);
                win.AddButton(FakeWindow.Result.Cancel);
            }
            else if (buttons == VikMessageBoxButtons.YESNO)
            {
                focusButton = win.AddButton(FakeWindow.Result.Yes);
                win.AddButton(FakeWindow.Result.No);
            }

            if (width != double.NaN)
                win.tbText.Width = width;

            var result = VikGame.ScreenManager.ShowContentModal(win, focusButton, true);
            return result;
        }
    }
}
