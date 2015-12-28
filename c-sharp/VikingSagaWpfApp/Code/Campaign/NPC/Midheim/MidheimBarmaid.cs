using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.NPC
{
    class MidheimBarmaid : QuestNPC
    {
        override public String CheckActions(Hero selectedHero)
        {
            if (selectedHero.CampaignType == CampaignFactory.CampaignEnum.TheBloodWolf)
            {
                if (selectedHero.Quests.Count(q => q.QuestID == QuestFactory.THE_MAD_BOAR) > 0)
                {
                    var quest = selectedHero.Quests.Single(q => q.QuestID == QuestFactory.THE_MAD_BOAR);
                    if (quest.Step < 2)
                        return "I can see that you still haven't killed the evil boar. Come back when you have done that for me.";
                    if (quest.Step == 2)
                    {
                        selectedHero.Gold += 20;
                        return "Oh thank you so much kind stranger for killing that horrible boar for me, finally my husband can rest in peace! Please take this gold, it is all I have.";
                    }
                }
                else
                {
                    selectedHero.Quests.Add(new QuestProgress { QuestID = QuestFactory.THE_MAD_BOAR, Step = 1 });
                    return "Please go kill the evil boar for me, it killed my husband";
                }
            }

            return null;
        }
    }
}
