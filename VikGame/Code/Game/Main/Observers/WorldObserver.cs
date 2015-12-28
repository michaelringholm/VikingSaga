using GameLib.Encounters;
using GameLib.Interface;
using GameLib.Quests;
using GameLib.World.Maps;
using System.Windows.Media;
using Vik.Code.Controls.Quests;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Game.Main.Observers
{
    public class WorldObserver : IWorldObserver
    {
        private Map _currentMap;

        void IWorldObserver.OnEnterMap(Map map)
        {
            MapChanged(map);
        }
        
        private void MapChanged(Map map)
        {
            _currentMap = map;
            var resourceName = GetResourceNameForMap(map);
            var mapImage = VikGame.ResourceLocator.GetImageResource(resourceName);

            VikGame.MapWindow.ChangeMap(map, mapImage);
        }

        private string GetResourceNameForMap(Map map)
        {
            // Ex. map class Cave1Map is expected to have a resource image 'Cave1Map.png'
            return string.Format("{0}/{1}.png", "Data/Gfx/Maps", map.GetType().Name);
        }

        void IWorldObserver.OnQuestAdded(Quest quest)
        {
            VikGame.Sound.Play("iQuestComplete.ogg");

            var win = new NewQuestWindow();
            win.SetQuest(quest);
            VikGame.ScreenManager.ShowContentModal(win, win.btnAccept, true);
        }

        public void OnQuestUpdated(Quest quest)
        {
            VikGame.Sound.Play("MapPing.ogg");
            UiUtil.ShowFloatingInfo("Quest updated: " + quest.GetTitle(), 0.5, 0.4, true, Colors.Yellow, false, 0, 0, -100, 30, 500, 3000, 2000);
        }

        public void OnQuestCompleted(Quest quest)
        {
            VikGame.Sound.Play("iQuestComplete.ogg");

            var win = new QuestCompletedWindow();
            win.SetQuest(quest);
            VikGame.ScreenManager.ShowContentModal(win, win.btnAccept, true);
            VikGame.World.PlayerProfile.ReceiveTreasure(quest.GetQuestReward());
        }

        public void OnQuestEncounterTriggered(Encounter encounter)
        {
            VikGame.EncounterController.RunEncounterAsync(encounter);
        }
    }
}
