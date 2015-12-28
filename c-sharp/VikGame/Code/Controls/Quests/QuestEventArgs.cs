using GameLib.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vik.Code.Controls.Quests
{
    public class QuestEventArgs : EventArgs
    {
        public Quest Quest { get; set; }
    }
}
