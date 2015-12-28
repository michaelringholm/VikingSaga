using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Quests
{
    public class QuestEventArgs : EventArgs
    {
        public enum QuestEventEnum { Null };
        public QuestEventEnum QuestEvent { get; set; }
    }
}
