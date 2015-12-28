using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface ILonghouseUI : IUIControl
    {
        void Show(Hero hero, Deck deck);
    }
}
