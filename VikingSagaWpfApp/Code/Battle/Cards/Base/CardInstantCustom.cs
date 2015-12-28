using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public class CardInstantCustom : CardInstant
    {
        public CardInstantCustom()
        {
            Name = "CardInstantCustom";
            ExecuteAction = () => { };
        }

        public void Execute()
        {
            ExecuteAction();
        }

        protected Action ExecuteAction { get; set; }
        public SpellProperty.Result Effect { get; protected set; }
    }
}
