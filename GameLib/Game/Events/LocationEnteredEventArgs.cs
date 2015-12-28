using GameLib.World.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Game
{
    public class LocationEnteredEventArgs
    {
        public Map Map { get; set; }
        public string LocationId { get; set; }
    }
}
