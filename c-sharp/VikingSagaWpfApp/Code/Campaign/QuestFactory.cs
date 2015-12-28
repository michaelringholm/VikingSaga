using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code.Campaign
{
    public static class QuestFactory
    {
        #region Quest IDs
        public const int THE_MAD_BOAR = 1;
        #endregion

        public enum QuestEnum { TheBeginning, TheMadBoar };
        public enum TriggerEnum { Barmaid, CampaignStart };

        public static Quest CreateQuest(QuestEnum questType)
        {
            if (questType == QuestEnum.TheBeginning)
            {
                var quest = new Quest();
                quest.Description = "The Sun is about to rise in the east and you stand in the middle of you home town Midheim. The air is fresh you feel strong and you are full of excitement. There is so much to discover out there. Adventures await and you think you are ready to go out beyond the safe walls of Midheim and explore the world.";
                quest.Trigger = TriggerEnum.CampaignStart;
                quest.Objective = "None";

                return quest;
            }
            if (questType == QuestEnum.TheMadBoar)
            {
                var quest = new Quest();
                quest.Description = "The Sun is about to rise in the east and you stand in the middle of you home town Midheim. The air is fresh you feel strong and you are full of excitement. There is so much to discover out there. Adventures await and you think you are ready to go out beyond the safe walls of Midheim and explore the world.";
                quest.Trigger = TriggerEnum.Barmaid;
                quest.Objective = "Talk to the barmaid";

                return quest;
            }
            else
                throw new Exception("Unknown quest");
        }

        internal static string GetTitle(int questId)
        {
            if (questId == THE_MAD_BOAR)
                return "The mad boar";
            else
                return "Unknown quest";
        }
    }
}
