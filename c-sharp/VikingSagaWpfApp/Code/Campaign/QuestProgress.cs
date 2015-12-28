using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code.Campaign
{
    public class QuestProgress
    {
        public int QuestID { get; set; }
        public int Step { get; set; }
        public String MetaData { get; set; }

        public string Title { get; set; }

        public string Objective { get; set; }
    }
}
