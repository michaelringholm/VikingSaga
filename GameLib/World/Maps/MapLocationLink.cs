using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace GameLib.World.Maps
{
    public class MapLocationLinkData
    {
        public string Node1Id { get; set; }
        public string Node2Id { get; set; }
        public readonly List<Point> Points = new List<Point>();
    }
}
