using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.NPC
{
    public abstract class QuestNPC
    {
        public abstract String CheckActions(Hero selectedHero);
    }
}
