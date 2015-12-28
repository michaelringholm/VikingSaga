using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingSaga.Code.Campaign;

namespace VikingSaga.Code
{
    public interface IQuestListUI : IUIControl
    {
        void Show(List<QuestProgress> quests);
    }
}
