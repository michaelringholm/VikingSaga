using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSaga.Code
{
    public interface IPreBattleUI
    {
        void Show(Encounter encounter);

        void Hide();
    }
}
