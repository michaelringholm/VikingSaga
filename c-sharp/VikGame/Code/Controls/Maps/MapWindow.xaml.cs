using System.Windows.Media;
using GameLib.World.Maps;
using Vik.Code.Utility;
using Vik.Code.Controls.Utility;
using System.Windows.Controls;
using Vik.Code.Controls.Player;
using System.Windows;
using Vik.Code.Controls.Cards;
using System;
using GameLib.DataStore.DTOs;
using System.Collections.Generic;

namespace Vik.Code.Controls.Maps
{
    public partial class MapWindow : FakeWindow
    {
        public MapWindow()
        {
            InitializeComponent();
            mapControl.tbDebugInfo.Visibility = VikGame.IsDeveloperMode ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void SetProfile(ProfileDTO profile)
        {
            var partyCards = VikGame.World.PlayerProfile.Data.PartyCards;
            for (int i = 0; i < partyCards.Length; ++i)
            {
                var ph = GetPartyPlaceholder(i);
                ph.SetCard(partyCards[i]);
            }
        }

        private CardPlaceholderControl GetPartyPlaceholder(int idx)
        {
            switch (idx)
            {
                case 0: return Party0;
                case 1: return Party1;
                case 2: return Party2;
                case 3: return Party3;
                case 4: return Party4;
                default: throw new ArgumentException("No party placeholder at idx: " + idx);
            }
        }

        public void ChangeMap(Map map, ImageSource imageSource)
        {
            mapControl.SetMap(map, imageSource);
        }

        public void SetPlayerLocation(MapLocation mapLocation)
        {
            mapControl.SetPlayerLocation(mapLocation);

            UiUtil.SetTextBlockText(tbLocation, mapLocation.Title);
            UiUtil.SetTextBlockText(tbDescription, mapLocation.Description);

           ActionButtons.ItemsSource = mapLocation.Actions;
        }

        private void btnLocationAction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = (Button)sender;
            var locationAction = (MapLocationAction)button.Tag;

            VikGame.World.ExecuteMapLocationAction(locationAction);
        }

        private void ButtonGameMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VikGame.ShowGameMenu();
        }

        private void ButtonBackpack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var backpackWin = new PlayerBackpackWindow();
            VikGame.ScreenManager.ShowContentModal(backpackWin, backpackWin.btnClose, true);
        }

        private void ButtonQuestLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var questLogWin = new PlayerQuestLogWindow();
            VikGame.ScreenManager.ShowContentModal(questLogWin, questLogWin.btnClose, true);
        }

        private void ButtonEditDeck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var editDeckWin = new PlayerEditCardsWindow();
            VikGame.ScreenManager.ShowContentModal(editDeckWin, editDeckWin.btnClose, true);
        }
    }
}
