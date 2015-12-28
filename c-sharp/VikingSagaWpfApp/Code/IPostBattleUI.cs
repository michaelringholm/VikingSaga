using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IPostBattleUI
    {
        void ShowYouWonScreen(Encounter encounter);
        void ShowYouLostScreen(Encounter encounter);

        void ShowLevelGained(Hero hero);
    }
}
