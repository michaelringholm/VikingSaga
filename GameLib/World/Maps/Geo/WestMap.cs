using GameLib.World.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.World.Maps.Geo
{
    public class WestMap : Map
    {
        public override string Id { get { return this.GetType().Name; } }
        public override string Title { get { return "West Map"; } }

        override protected void AddGameLogicToLocations()
        {
            AddChangePlayerLocationAction("ToStart", "Go Start", "StartMap", "ToWest");
        }
    }
}
