using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface ICreateHeroUI : IUIControl
    {
        void Update(VikingSagaUserProfile Profile);
    }
}
