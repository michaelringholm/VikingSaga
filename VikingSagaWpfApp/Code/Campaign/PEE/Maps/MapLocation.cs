using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public class MapLocationData
    {
        public string Id { get; set; }
        public Point Location { get; set; }
    }

    public class MapLocationPEE : MapLocationData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public readonly List<MapLocationLinkData> Links = new List<MapLocationLinkData>();

        public Action BeforeEnter = () => { };
        public Action Enter = () => { };
        public Action BeforeLeave = () => { };
        public Action Leave = () => { };

        public MapSpecialLocation SpecialLocation { get; set; }

        public MapLocationPEE(MapLocationData data)
        {
            this.Id = data.Id;
            this.Location = data.Location;
        }
    }
}
