using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IValkyrieUI : IUIControl
    {
        void Show(Hero hero, Deck deck);
    }
}
