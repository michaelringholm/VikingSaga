using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IHeroHomeUI : IUIControl
    {
        void Update(Hero hero, Deck deck);
    }
}
