using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public class PathHelper
    {
        public string LocationFromId { get; private set; }
        public string LocationToId { get; private set; }

        public List<Point> FixPoints { get; private set; }
        public double TotalDistance { get; private set; }

        private MapLocationLinkData _link;

        public PathHelper(string locationFromId, string locationToId, MapLocationLinkData link, bool reverse)
        {
            LocationFromId = locationFromId;
            LocationToId = locationToId;
            _link = link;

            FixPoints = new List<Point>();
            FixPoints.AddRange(link.Points);
            if (reverse)
            {
                FixPoints.Reverse();
            }

            TotalDistance = GetTotalDistance();
        }

        public double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(DistanceSquared(a, b));
        }

        public double GetDistance(int idx1, int idx2)
        {
            return GetDistance(FixPoints[idx1], FixPoints[idx2]);
        }

        public Point GetPoint(double t)
        {
            if (t <= 0)
                return FixPoints[0];

            if (t >= 1)
                return FixPoints[FixPoints.Count - 1];

            double distanceTarget = t * TotalDistance;
            double distanceToNext;
            double distanceAtCurrent = 0;
            int currentIdx = 0;

            while (true)
            {
                distanceToNext = GetDistance(currentIdx, currentIdx + 1);
                if (distanceAtCurrent + distanceToNext > distanceTarget || currentIdx == FixPoints.Count - 2)
                    break; // One more point would be too far

                distanceAtCurrent += distanceToNext;
                currentIdx++;
            }

            double localT = distanceTarget - distanceAtCurrent;
            var result = GetPointBetween(FixPoints[currentIdx], FixPoints[currentIdx + 1], localT);
            return result;
        }

        private Point GetPointBetween(Point a, Point b, double t)
        {
            double tx = a.X + ((b.X - a.X) * t);
            double ty = a.Y + ((b.Y - a.Y) * t);
            return new Point(tx, ty);
        }

        private double DistanceSquared(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        private double GetTotalDistance()
        {
            double result = 0.0;
            for (int i = 0; i < FixPoints.Count - 1; ++i)
            {
                double distance = GetDistance(i, i + 1);
                result += distance;
            }
            return result;
        }
    }
}
