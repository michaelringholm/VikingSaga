using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public interface IMapUI : IUIControl
    {
        void DrawMap(Map map, Hero hero);        

        void ShowMap();

        void ShowInvalidHeroMoveEffect(MapLocation newMapLocation);

        void MarkHeroLocation(MapLocation mapLocation);

        void DrawNewHeroMapPosition(MapLocation oldMapLocation, MapLocation newMapLocation);
    }
}
