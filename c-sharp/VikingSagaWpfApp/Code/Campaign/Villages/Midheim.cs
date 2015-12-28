using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.NPC
{
    class Midheim : Village
    {

        public override void EnterVillage(Hero selectedHero)
        {            
            if (selectedHero.CampaignType == CampaignFactory.CampaignEnum.TheBloodWolf)
            {
                if (selectedHero.Quests != null && selectedHero.Quests.Count(q => q.QuestID == QuestFactory.THE_MAD_BOAR) > 0)
                {
                    var quest = selectedHero.Quests.Single(q => q.QuestID == QuestFactory.THE_MAD_BOAR);
                    if (quest.Step < 2)
                    {
                        GameController.Current.ShowVillage("The barmaid waves at you eagerly.");
                        return;
                    }
                    if (quest.Step == 2)
                    {
                        selectedHero.Gold += 20;
                        GameController.Current.ShowVillage("You feel people staring at you in the streets, and you hear whispers calling you the great Boar slayer.");
                        return;
                    }
                }
                else
                {
                    GameController.Current.ShowVillage("Your hear rumors on the street that the barmaid is having problems with some sort of creature. Maybe you should visit the longhouse.");
                    return;
                }
            }
            
            GameController.Current.ShowVillage();
        }

        public override void EnterLonghouse(Hero selectedHero)
        {
            if (selectedHero.CampaignType == CampaignFactory.CampaignEnum.TheBloodWolf)
            {
                if (selectedHero.Quests.Count(q => q.QuestID == QuestFactory.THE_MAD_BOAR) > 0)
                {
                    var quest = selectedHero.Quests.Single(q => q.QuestID == QuestFactory.THE_MAD_BOAR);
                    if (quest.Step < 2)
                    {
                        GameController.Current.ShowLonghouse("I can see that you still haven't killed the evil boar. Come back when you have done that for me.", false);
                        return;
                    }
                    if (quest.Step == 2)
                    {
                        selectedHero.Gold += 20;
                        GameController.Current.ShowLonghouse("Oh thank you so much kind stranger for killing that horrible boar for me, finally my husband can rest in peace! Please take this gold, it is all I have.", false);
                        return;
                    }
                }
                else
                {
                    var quest = new QuestProgress { QuestID = QuestFactory.THE_MAD_BOAR, Step = 1, Title = QuestFactory.GetTitle(QuestFactory.THE_MAD_BOAR), Objective = "Kill the evil boar" };
                    GameEngine.Current.PendingQuest = quest;
                    GameController.Current.ShowLonghouse("Please go kill the evil boar for me, it killed my husband, it was last seen in the hills to the west of town.", true);
                    return;
                }
            }

            GameController.Current.ShowLonghouse();
        }

        public override void EnterSmithy(Hero selectedHero)
        {
            GameController.Current.ShowSmithHouse();
        }

        public override void EnterValkyrieGraveyard(Hero selectedHero)
        {
            GameController.Current.ShowValkyrieGraveyard();
        }

        public override void EnterSeersHut(Hero selectedHero)
        {
            GameController.Current.ShowSeerHut();
        }

        public override void EnterMerchantsShop(Hero selectedHero)
        {
            GameController.Current.ShowMerchantShop();
        }
    }
}
