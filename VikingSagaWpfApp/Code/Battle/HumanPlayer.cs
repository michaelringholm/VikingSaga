using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle
{
    class HumanPlayer : Player
    {
        public override void TakeTurn(Battle battle)
        {
            DrawFromDeck();
            Battle.Observer.WaitForHumanDecisions();
        }
    }
}
