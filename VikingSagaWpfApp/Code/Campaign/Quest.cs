using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VikingSaga.Code.Campaign
{
    public class Quest
    {
        public string Description { get; set; }

        [XmlIgnore]
        public QuestFactory.TriggerEnum Trigger { get; set; }

        public string Objective { get; set; }

        /* Serialization helper as enums can't be serialized */
        public String TriggerString 
        {
            get { return Trigger.ToString(); }
            set { Trigger = (QuestFactory.TriggerEnum)Enum.Parse(typeof(QuestFactory.TriggerEnum), TriggerString); }
        }
    }
}
