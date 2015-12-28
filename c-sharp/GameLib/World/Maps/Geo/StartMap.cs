using GameLib.Encounters;
using GameLib.Quests;
using System;
using System.Collections.Generic;
using Vik.Code.Game.Main.Profiles;

namespace GameLib.World.Maps.Geo
{
    public class StartMap : Map
    {
        public override string Id { get { return this.GetType().Name; } }
        public override string Title { get { return "Start Map"; } }

        protected override void InitTravelEncounters()
        {
            var travelEncounter = new TravelEncounter(0.5, 0.5, "LakeLeechesHide", "TheBanditKingsLair", EncounterWeightFactory.Create(EncounterFactory.EncounterEnum.WolfPack1, 1, _gameEventManager));
            AddTravelEncounter(travelEncounter);            
        }

        protected override void InitLocationEncounters()
        {
        }

        override protected void AddGameLogicToLocations()
        {
            SetLocationText("Midheim", "Outside Midheim", "Hello <B>world</B>. This is the description. Blah blah. You are outside some town. Yup. You can enter if you want to.");

            //AddChangePlayerLocationAction("ToWest", "Go West", "WestMap", "ToStart");
            AddChangePlayerLocationAction("TheBanditKingsLair", "Enter Lair", "SouthMap", "ToStart");
            AddChangePlayerLocationAction("TheBottomlessWell", "Enter Cave", "Cave1Map", "Exit");

            AddEnterSpecialAction("Midheim", "Enter Midheim", WorldData.SpecialLocationEnum.Midheim);
        }
    }
}
