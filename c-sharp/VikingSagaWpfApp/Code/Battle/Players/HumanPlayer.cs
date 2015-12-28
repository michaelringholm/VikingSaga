using System;

namespace VikingSagaWpfApp.Code.BattleNs
{
    class HumanPlayer : Player
    {
        public override void TakeTurn()
        {
            Observer.WaitForHumanDecisions();
        }
    }
}
