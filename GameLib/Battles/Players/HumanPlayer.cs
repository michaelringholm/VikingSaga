using System;

namespace GameLib.Battles
{
    public class HumanPlayer : Player
    {
        public override void TakeTurn()
        {
            Observer.WaitForHumanEndTurn();
        }
    }
}
