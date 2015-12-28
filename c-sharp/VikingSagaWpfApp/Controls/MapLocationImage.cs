using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VikingSaga.Code;

namespace VikingSagaWpfApp.Controls
{
    public class MapLocationImage : Image
    {
        public MapLocationImage()
        {
            
        }

        public MapLocation MapLocation { get; set; }
    }
}
