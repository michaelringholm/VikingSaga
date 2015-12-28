using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSagaWpfApp.Code
{
    public interface IMainWindowUI
    {
        void ChangeBodyContent(VikingSaga.Code.IUIControl BattleBoard);

        void Close();

        void DisableNonCombatButtons();

        void EnableNonCombatButtons();
    }
}
