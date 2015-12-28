using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    class RealPlayer // : Player
    {
        internal void MakeDecision() // virtual
        {
            GameEngine.Observer.WaitForDecisions();
        }
    }
}
