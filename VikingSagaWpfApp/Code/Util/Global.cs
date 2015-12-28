using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfTest;

namespace Global
{
    // To initialize game:
    //
    // 1) Replace StartupUri in app.xaml with a Startup handler that calls 'Global.ApplicationStartup'
    // 2) Done.

    public class Game
    {
        private static readonly Game Instance;

        static Game()
        {
            Instance = new Game();
        }

        public static void ApplicationStartup()
        {
            InitializeScreenManager();
        }

        public static ScreenManager ScreenManager { get; private set; }

        private static void InitializeScreenManager()
        {
            //ScreenManager = new ScreenManager(mainWindow);

            //ScreenManager.PushContent(new Full1());
            //ScreenManager.PushContent(new Full2());
            //ScreenManager.PopContent();
            //ScreenManager.PushContent(new Center());
        }
    }
}
