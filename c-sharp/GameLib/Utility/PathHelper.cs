using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using GameLib.World.Maps;

namespace GameLib.Utility
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

            TotalDistance = CalcTotalDistance(FixPoints);
        }

        public IEnumerable<Point> CalcFixPoints(double tBegin = 0.0, double tEnd = 1.0)
        {
            const double Step = 0.02; // 50 points on a link

            double t = tBegin;
            while (t < tEnd)
            {
                yield return GetPoint(FixPoints, t);
                t += Step;
            }

            yield return GetPoint(FixPoints, tEnd);
        }

        public static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(DistanceSquared(a, b));
        }

        public static double GetDistance(List<Point> points, int idx1, int idx2)
        {
            return GetDistance(points[idx1], points[idx2]);
        }

        public static Point GetPoint(List<Point> points, double t)
        {
            if (t <= 0)
                return points[0];

            if (t >= 1)
                return points[points.Count - 1];

            double distanceTarget = t * CalcTotalDistance(points);
            double distanceToNext;
            double distanceAtCurrent = 0;
            int currentIdx = 0;

            while (true)
            {
                distanceToNext = GetDistance(points, currentIdx, currentIdx + 1);
                if (distanceAtCurrent + distanceToNext > distanceTarget || currentIdx == points.Count - 2)
                    break; // One more point would be too far

                distanceAtCurrent += distanceToNext;
                currentIdx++;
            }

            double localT = distanceTarget - distanceAtCurrent;
            var result = GetPointBetween(points[currentIdx], points[currentIdx + 1], localT);
            return result;
        }

        private static Point GetPointBetween(Point a, Point b, double t)
        {
            double tx = a.X + ((b.X - a.X) * t);
            double ty = a.Y + ((b.Y - a.Y) * t);
            return new Point(tx, ty);
        }

        private static double DistanceSquared(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static double CalcTotalDistance(List<Point> points)
        {
            double result = 0.0;
            for (int i = 0; i < points.Count - 1; ++i)
            {
                double distance = GetDistance(points, i, i + 1);
                result += distance;
            }
            return result;
        }
    }
}
