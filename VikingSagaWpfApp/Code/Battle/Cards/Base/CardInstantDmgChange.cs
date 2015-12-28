using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public class CardInstantDmgChange : CardInstant
    {
        public CardInstantDmgChange()
        {
            Name = "CardInstantDmgChange";
        }

        public int Amount { get; protected set; }
        public string OnApplyMsg { get; protected set; }
    }
}
