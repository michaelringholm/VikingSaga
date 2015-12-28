using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VikingSagaWpfApp.Controls;

namespace VikingSagaWpfApp.Animations
{
    public static class EffectHelper
    {
        public static RipControl ApplyRipEffect(Grid grid)
        {
            RipControl control = new RipControl();
            grid.Children.Add(control);
            control.Run(grid);
            return control;
        }

        public static void CloseRipEffect(RipControl rip)
        {
            rip.Close();
        }
    }
}
