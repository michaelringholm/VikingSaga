using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IProfileUI : IUIControl
    {
        void Show(VikingSagaUserProfile profile);
        void UpdateProfileDetails(VikingSagaUserProfile profile);
    }
}
