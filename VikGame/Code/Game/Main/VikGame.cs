using System;
using System.Threading;
using System.Windows;
using Vik.Code.Application;
using Vik.Code.Controls.Base;
using Vik.Code.Controls.Login;
using Vik.Code.Utility;
using Vik.Code.Game.Main.Observers;
using Vik.Code.Controls.Maps;
using Vik.Code.Controls.Utility;
using GameLib.Interface;
using GameLib.World;
using GameLib.DataStore;
using GameLib.Game;
using System.Windows.Media;
using Vik.Code.Game.Main.Battle;

namespace Vik
{
    public class VikGame : IGameDataStoreErrorCallback
    {
        public static readonly bool IsDeveloperMode = true;

        public static EncounterController EncounterController { get { return Instance._encounterController; } }
        public static ScreenManager ScreenManager { get; private set; }
        public static IProfileManager ProfileManager { get { return Instance._profileManager; } }
        public static MapWindow MapWindow { get { return Instance._mapWindow; } }
        public static readonly Sound Sound = new Sound();

        public static IWorld World { get { return Instance._world; } }

        public static MapObserver MapObserver { get { return Instance._mapObserver; } }
        public static WorldObserver WorldObserver { get { return Instance._worldObserver; } }

        public static ResourceLocator ResourceLocator { get; private set; }
        public static ResourceChecker ResourceChecker { get; private set; } // For debugging

        private static readonly VikGame Instance;

        private EncounterController _encounterController = new EncounterController();
        private IWorld _world;
        private WorldObserver _worldObserver;
        private MapObserver _mapObserver;

        private ProfileManagerLocalData _profileManager;

        private MapWindow _mapWindow;

        static VikGame()
        {
            Instance = new VikGame();
        }

        public static void Exit()
        {
            ScreenManager.MainWindow.Close();
        }

        public static void ApplicationStartup()
        {
            UiUtil.Init(SynchronizationContext.Current);

            InitializeScreenManager();

            ResourceLocator = new ResourceLocator();
            ResourceChecker = new ResourceChecker(""); // TODO: SiteOfOriginPath

            Instance.InitializeGame();
        }

        private static void InitializeScreenManager()
        {
            var mainWindow = new MainWindow();
            ScreenManager = new ScreenManager(mainWindow);
            mainWindow.KeyDown += mainWindow_KeyDown;
            mainWindow.PreviewKeyDown += mainWindow_PreviewKeyDown;
            mainWindow.Closed += mainWindow_Closed;
        }

        static void mainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.R && e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                ToggleFps();
            }
            else if (IsDeveloperMode && e.Key == System.Windows.Input.Key.T && e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                ToggleKeyValueInfoWindow();
            }
        }

        static void mainWindow_Closed(object sender, EventArgs e)
        {
            if (World != null)
                World.SaveAll();
        }

        private static KeyValueInfoWindow s_keyValueInfoWindow;

        static void mainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                ShowGameMenu();
                e.Handled = true;
            }
        }

        private static void ToggleKeyValueInfoWindow()
        {
            if (s_keyValueInfoWindow == null)
            {
                s_keyValueInfoWindow = new KeyValueInfoWindow();
                s_keyValueInfoWindow.Owner = ScreenManager.MainWindow;
                s_keyValueInfoWindow.Show();

                ScreenManager.MainWindow.Focus();
            }
            else
            {
                s_keyValueInfoWindow.Close();
                s_keyValueInfoWindow = null;
            }
        }

        private static void ToggleFps()
        {
            var tbFps = ScreenManager.MainWindow.tbFps;
            var border = ScreenManager.MainWindow.fpsBorder;
            bool wasEnabled = border.Visibility == Visibility.Visible;
            border.Visibility = wasEnabled ? Visibility.Hidden : Visibility.Visible;
            if (wasEnabled)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
            else
            {
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        private static double _lastRenderTime;
        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var renderArgs = (RenderingEventArgs)e;
            double currentRenderTime = renderArgs.RenderingTime.TotalMilliseconds;
            // See http://stackoverflow.com/questions/5812384/why-is-frame-rate-in-wpf-irregular-and-not-limited-to-monitor-refresh
            if (currentRenderTime == _lastRenderTime)
                return;

            double fps = 1000 / (currentRenderTime - _lastRenderTime);
            _lastRenderTime = renderArgs.RenderingTime.TotalMilliseconds;
            ScreenManager.MainWindow.tbFps.Text = string.Format("{0} fps", (int)fps);
        }

        public static void ShowGameMenu()
        {
            var gameMenu = new GameMenuWindow();
            ScreenManager.ShowContentModal(gameMenu, gameMenu.Border, true);
        }

        private void InitializeGame()
        {
            _profileManager = new ProfileManagerLocalData();

            ScreenManager.MainWindow.fpsBorder.Visibility = Visibility.Hidden;
            ScreenManager.MainWindow.tbDebug.Visibility = IsDeveloperMode ? Visibility.Visible : Visibility.Hidden;
            if (IsDeveloperMode)
            {
                UiUtil.ShowFloatingInfo("Developer mode is ON", ScreenManager.XPos(0.5), ScreenManager.YPos(0.85), true, Colors.Red, true, 1000, 1, 10, 30, 200, 4000, 1000);
            }

            LoginWindow loginWindow = new LoginWindow();
            if (VikGame.ScreenManager.ShowContentModal(loginWindow) != Code.Controls.FakeWindow.Result.OK)
            {
                VikGame.Exit();
                return;
            }

            _mapWindow = new MapWindow();
            ScreenManager.PushWindow(_mapWindow);

            InitializeWorld(loginWindow.SelectedProfile);

            _mapWindow.SetProfile(World.PlayerProfile.Data);

            UiUtil.ShowLargeOverlayText("Test text. <B>Bold</B> <C ORANGE>Orange!");
        }

        private void InitializeWorld(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                throw new ArgumentException("Cannot start world, profile name is empty");

            string saveFile = Util.GetStoreFileForProfile(profileName);
            IGameDataStore dataStore = XmlFileGameDataStore.LoadOrCreate(saveFile, this);

            var loadErrorInfo = dataStore.GetLoadErrors();
            if (!string.IsNullOrWhiteSpace(loadErrorInfo))
                MessageBox.Show(loadErrorInfo, "Error loading save file", MessageBoxButton.OK, MessageBoxImage.Error);

            _worldObserver = new WorldObserver();
            _mapObserver = new MapObserver();

            GlobalData globalData = new GlobalData();
            globalData.DataStore = dataStore;
            globalData.WorldObserver = _worldObserver;
            globalData.MapObserver = _mapObserver;

            _world = WorldFactory.Create(globalData);
            _world.Run();
            _world.PlayerProfile.Data.Name = profileName;
        }

        void IGameDataStoreErrorCallback.OnValueError(string data, System.Exception e)
        {
            string selectedMessage = e.InnerException == null ? e.Message : e.InnerException.Message;
            data = FormattedTextParser.EncodeBeginEndTags(data);
            data = data.Replace(Environment.NewLine, "<BR>");

            string message = FormattedTextParser.EncodeBeginEndTags(selectedMessage);
            message = message.Replace(Environment.NewLine, "<BR>");

            UiUtil.VikMessageBox(string.Format("<F+>DataStore error<F-><BR><BR>{0}<BR><BR>{1}", message, data), VikMessageBoxButtons.OK, 800);
        }
    }
}
