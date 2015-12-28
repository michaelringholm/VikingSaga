using GameLib.World;
using GameLib.World.Maps;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Vik.Code.Controls.Utility;
using Vik.Code.Controls.Utility.Debug;
using Vik.Code.Utility;
using System.Windows.Controls;
using System.Diagnostics;

namespace Vik.Code.Controls.Base
{
    internal class GameMenuItem
    {
        public string Display { get; set; }
        public Action ClickAction { get; set; }
    }

    public partial class GameMenuWindow : FakeWindow
    {
        internal readonly List<GameMenuItem> MenuItems = new List<GameMenuItem>();

        public GameMenuWindow()
        {
            InitializeComponent();

            // Work-around to get data in the designer. Remove the design buttons here and add the real data.
            Profiles.Items.Clear();
            Profiles.ItemsSource = MenuItems;

            AddButtons();

            Loaded += delegate { AnimHelper.ApplyPopInAnimation(this); };
        }

        private void AddButtons()
        {
            MenuItems.Add(new GameMenuItem { Display = "Exit Game", ClickAction = ActionExit });
            if (VikGame.IsDeveloperMode)
            {
                MenuItems.Add(new GameMenuItem { Display = "Jump To Map (Debug)", ClickAction = ActionJumpToMap });
                MenuItems.Add(new GameMenuItem { Display = "Text Editor (Debug)", ClickAction = ActionTextEditor });
                MenuItems.Add(new GameMenuItem { Display = "View Save Folder (Debug)", ClickAction = ViewSaveFolder });
            }
            MenuItems.Add(new GameMenuItem { Display = "Back", ClickAction = ActionBack });
        }

        private void ViewSaveFolder()
        {
            Process.Start(Util.GetFolderForLocalData());
        }

        private void ActionTextEditor()
        {
            var win = new DebugTestFormattedText();
            VikGame.ScreenManager.ShowContentModal(win, win.btnClose, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            var clickedItem = this.MenuItems.Where(item => item == tag).FirstOrDefault();
            clickedItem.ClickAction();
        }

        private void ActionExit()
        {
            VikGame.Exit();
        }

        private void ActionBack()
        {
            Close(Result.NotSet);
        }

        private void ActionJumpToMap()
        {
            var win = new DebugSelectItemWindow();
            if (VikGame.World == null)
            {
                UiUtil.VikMessageBox("World not created yet", VikMessageBoxButtons.OK);
                return;
            }

            win.DataContext = VikGame.World.GetAllMaps();

            if (VikGame.ScreenManager.ShowContentModal(win, null, true) == Result.OK)
            {
                Map map = win.Selection;
                VikGame.World.ChangePlayerLocation(map.Id, map.Locations[0].Id, PlayerChangeLocationMethod.Port);
            }
        }
    }
}
