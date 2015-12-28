using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VikingSaga.Code.Campaign
{
    public static class CampaignFactory
    {
        public enum CampaignEnum { TheBloodWolf };

        public static Campaign CreateCampaign(CampaignEnum campaignType)
        {
            if (campaignType == CampaignEnum.TheBloodWolf)
            {
                var campaign = new Campaign();
                campaign.Quests.Add(QuestFactory.CreateQuest(QuestFactory.QuestEnum.TheBeginning));
                campaign.Quests.Add(QuestFactory.CreateQuest(QuestFactory.QuestEnum.TheMadBoar));                

                return campaign;
            }
            else
                throw new Exception("Unknown campaign");
        }
    }
}
