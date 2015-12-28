using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign
{
    public class Campaign
    {
        public Campaign()
        {
            Quests = new List<Quest>();
        }

        public List<Quest> Quests { get; set; }
    }
}
