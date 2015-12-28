using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign.NPC
{
    public abstract class Village
    {
        public Village()
        {

        }

        /*public abstract String CheckBarmaidActions();
        public abstract String CheckSmithActions();
        public abstract String CheckValkyrieActions();*/
        public abstract void EnterVillage(Hero selectedHero);
        public abstract void EnterLonghouse(Hero selectedHero);
        public abstract void EnterSmithy(Hero selectedHero);
        public abstract void EnterValkyrieGraveyard(Hero selectedHero);
        public abstract void EnterSeersHut(Hero selectedHero);
        public abstract void EnterMerchantsShop(Hero selectedHero);
    }
}
