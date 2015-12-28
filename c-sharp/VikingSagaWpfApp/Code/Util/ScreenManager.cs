using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Global
{
    public class ScreenManager
    {
        // MainWindow should not be resizable
        // Can set fullscreen or windowed in certain aspect ratios
        public Window MainWindow { get; private set; }

        private Grid _mainGrid;

        public ScreenManager(Window mainWindow)
        {
            MainWindow = mainWindow;
            _mainGrid = MainWindow.FindName("MainGrid") as Grid;
            if (_mainGrid == null)
                throw new ArgumentException("main window must contain a Grid named 'MainGrid'");

            MainWindow.KeyDown += MainWindow_KeyDown;
            SetSizeInAspectRatio(1.0, MainWindow);
            MainWindow.Show();
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

        public void PushContent(UIElement element)
        {
            _mainGrid.Children.Add(element);
            element.Visibility = Visibility.Visible;
        }

        public void PopContent()
        {
            int childCount = _mainGrid.Children.Count;
            _mainGrid.Children.RemoveAt(childCount - 1);
        }
    }
}
