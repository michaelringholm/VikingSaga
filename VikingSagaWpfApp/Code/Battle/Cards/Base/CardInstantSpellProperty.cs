using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public class CardInstantSpellProperty : CardInstant
    {
        public CardInstantSpellProperty()
        {
            Name = "CardInstantSpellProperty";
        }

        public SpellProperty Property { get; protected set; }
        public string OnApplyMsg { get; protected set; }

        public SpellProperty.Result Effect
        {
            get
            {
                return Property == null ? SpellProperty.Result.Unknown : Property.Effect;
            }
        }
    }
}
