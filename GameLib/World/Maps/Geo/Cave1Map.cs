using GameLib.World.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.World.Maps.Geo
{
    public class Cave1Map : Map
    {
        public override string Id { get { return this.GetType().Name; } }
        public override string Title { get { return "A Cave"; } }

        override protected void AddGameLogicToLocations()
        {
            AddChangePlayerLocationAction("Exit", "Leave Cave", "StartMap", "TheBottomlessWell");
        }
    }
}
