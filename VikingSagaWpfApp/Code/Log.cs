using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VikingSagaWpfApp.Windows;

namespace VikingSagaWpfApp
{
    public static class Log
    {
        private static LogWindow _window;
        private static SynchronizationContext _uiCtx;

        public static void CloseLog()
        {
            if (_window == null)
                return;

            _window.Close();
        }

        public static void Init(SynchronizationContext uiCtx)
        {
            _uiCtx = uiCtx;
            uiCtx.Send((x) =>
                {
                    _window = new LogWindow();
                    _window.Left = 0;
                    _window.Top = 0;
                    _window.Show();
                }, null);
        }

        public static void Line(string line)
        {
            line = string.Format("{0}> {1}{2}", DateTime.Now.ToString("HH:mm:ss.fff"), line, Environment.NewLine);
            Debug.Write(line);

            if (_uiCtx == null)
                return;

            _uiCtx.Send((x) =>
                {
                    _window.tbLog.Text = line + _window.tbLog.Text;
                }, null);
        }
    }
}
