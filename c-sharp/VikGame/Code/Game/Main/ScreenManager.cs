using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using Vik.Code.Application;
using Vik.Code.Controls;
using Vik.Code.Utility;

namespace Vik
{
    public class ScreenManager
    {
        // MainWindow should not be resizable
        // Can set fullscreen or windowed in certain aspect ratios
        public MainWindow MainWindow { get; private set; }

        public DragDropManager DragDropManager { get { return _dragDropManager; } }

        private Grid _mainGrid;
        private DragDropManager _dragDropManager = new DragDropManager();

        public ScreenManager(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            _mainGrid = MainWindow.MainGrid;

            MainWindow.KeyDown += MainWindow_KeyDown;
            SetSizeInAspectRatio(1.0, MainWindow);

            MainWindow.Closing += MainWindow_Closing;
            MainWindow.Show();
        }

        public double XPos(double pos)
        {
            return MainWindow.ActualWidth * pos;
        }

        public double YPos(double pos)
        {
            return MainWindow.ActualHeight * pos;
        }

        // Adorner layers:
        // Note that when requesting a layer for a control the framework searches up in the tree and returns the first layer found.
        public AdornerLayer GetGlobalAdornerLayer()
        {
            var layer = AdornerLayer.GetAdornerLayer(_mainGrid);
            return layer;
        }

        private void SetSizeInAspectRatio(double pct, Window window)
        {
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.ResizeMode = ResizeMode.NoResize;
            window.WindowState = WindowState.Normal;

            IntPtr windowHandle = new WindowInteropHelper(MainWindow).Handle;
            var screen = Screen.FromHandle(windowHandle);
            int sw = screen.WorkingArea.Width;
            int sh = screen.WorkingArea.Height;

            // Always let width determine window size. A screen turned 90 degrees should not get a tall window
            double ratio = sw > sh ?
                sh / (double)sw :
                sw / (double)sh;

            window.Width = (int)(sw * pct);
            window.Height = (int)(window.Width * ratio);
            window.Top = screen.WorkingArea.Top + (sh - window.Height) / 2;
            window.Left = screen.WorkingArea.Left + (sw - window.Width) / 2;
        }

        private void SetFullScreen(Window window)
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowState = WindowState.Maximized;
        }

        void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
            {
                SetFullScreen(MainWindow);
            }
            else if (e.Key == System.Windows.Input.Key.F2)
            {
                SetSizeInAspectRatio(1, MainWindow);
            }
            else if (e.Key == System.Windows.Input.Key.F3)
            {
                SetSizeInAspectRatio(0.8, MainWindow);
            }
            else if (e.Key == System.Windows.Input.Key.F4)
            {
                SetSizeInAspectRatio(0.5, MainWindow);
            }
        }

        private void PopAll()
        {
            while (_mainGrid.Children.Count > 0)
                PopWindow();
        }

        private void SetHitTestToTopOnly()
        {
            int count = _mainGrid.Children.Count;
            if (count == 0)
                return;

            for (int i = 0; i < count; ++i)
            {
                bool isTopMost = (i == count - 1);
                _mainGrid.Children[i].IsHitTestVisible = isTopMost;
            }
        }

        void element_CheckEscape(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                PopIfTopMost((FakeWindow)sender);
                e.Handled = true;
            }
        }

        private FakeWindow GetTopmost()
        {
            int childCount = _mainGrid.Children.Count;
            var result = _mainGrid.Children[childCount - 1];
            return (FakeWindow)result;
        }

        private void PopIfTopMost(FakeWindow window)
        {
            int childCount = _mainGrid.Children.Count;

            var topMost = GetTopmost();
            if (window == topMost)
                PopWindow();
        }

        private Stack<DispatcherFrame> _dispatcherFrameStack = new Stack<DispatcherFrame>();
        private Stack<ModalDialogData> _modalWindowDataStack = new Stack<ModalDialogData>();

        private class ModalDialogData
        {
            public FakeWindow _window;
            public UIElement _focusElement;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Prevent closing of main window until all modals are closed.
            if (_modalWindowDataStack.Count > 0)
            {
                e.Cancel = true;
                StopModalLoop();

                _doShutdown = true;
            }
        }

        private bool _doShutdown;

        private FakeWindow.Result RunModalLoop(FakeWindow window, UIElement focusElement)
        {
            var newFrame = new DispatcherFrame();
            _dispatcherFrameStack.Push(newFrame);

            var modalDialogData = new ModalDialogData { _window = window, _focusElement = focusElement };
            _modalWindowDataStack.Push(modalDialogData);

            // Blocking message pump until StopModalLoop is called when the modal window visibility changes to hidden
            Dispatcher.PushFrame(newFrame);

            // Remove own data from stack
            _modalWindowDataStack.Pop();

            if (_modalWindowDataStack.Count > 0)
            {
                // Restore focus on previous window
                var previousData = _modalWindowDataStack.Peek();
                AttemptSetFocusAsync(previousData._focusElement);
            }

            if (_doShutdown)
            {
                // If shutting down, keep trying to close MainWindow. Each attempt closes one modal window, until no more remains and MainWindow closes.
                MainWindow.Close();
            }

            return window.DialogResult;
        }

        private void StopModalLoop()
        {
            if (_dispatcherFrameStack.Count > 0)
            {
                var topDispatcherFrame = _dispatcherFrameStack.Pop();
                topDispatcherFrame.Continue = false; // This will cause Dispatcher.PushFrame(newFrame) in RunModalLoop to exit
            }
        }

        void ModalWindowVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                StopModalLoop();
            }
        }

        // Add blocking on top of everything else. Returns when content visible = false.
        public FakeWindow.Result ShowContentModal(FakeWindow window, UIElement focusElement = null, bool closeOnEscape = false)
        {
            PushWindow(window, focusElement, closeOnEscape);
            window.IsVisibleChanged += ModalWindowVisibilityChanged;

            var result = RunModalLoop(window, focusElement);
            return result;
        }

        // Replaces everything else
        public void SetWindow(FakeWindow window, UIElement focusElement = null)
        {
            PopAll();
            PushWindow(window, focusElement);
        }

        // Add on top of everything else
        public void PushWindow(FakeWindow window, UIElement focusElement = null, bool popOnEscape = false)
        {
            _mainGrid.Children.Add(window);

            if (popOnEscape)
            {
                window.PreviewKeyDown += element_CheckEscape;
            }

            window.Visibility = Visibility.Visible;
            window.IsVisibleChanged += element_CheckHidden;
            if (focusElement != null)
            {
                AttemptSetFocusAsync(focusElement);
            }

            SetHitTestToTopOnly();
        }

        // Remove top-most element
        public void PopWindow()
        {
            var elementToRemove = GetTopmost();
            elementToRemove.Visibility = Visibility.Hidden;
            _mainGrid.Children.Remove(elementToRemove);
            SetHitTestToTopOnly();
        }

        void element_CheckHidden(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                PopIfTopMost((FakeWindow)sender);
            }
        }

        private void AttemptSetFocus(UIElement element)
        {
            if (element != null)
                element.Focus();
        }

        private void AttemptSetFocusAsync(UIElement element)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action<UIElement>)AttemptSetFocus, DispatcherPriority.ContextIdle, element);
        }
    }
}
