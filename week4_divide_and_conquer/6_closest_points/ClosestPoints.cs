using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClosestPoints
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            /*
            var points = new List<Point>
            {
                P(4, 4),
                P(-2, -2),
                P(-3, -4),
                P(-1, 3),
                P(2, 3),
                P(-4, 0),
                P(1, 1),
                P(-1, -1),
                P(3, -1),
                P(-4, 2),
                P(-2, 4)
            };
            */
            
            var points = new List<Point>
            {
                P(7, 7),
                P(1, 100),
                P(-2, 100),
                P(4, 8),
                P(7, 7),
                P(8, 8),
                P(996, -100),
                P(1000, -100),
            };
#else
            List<Point> points;
            ParseInputs(out points);
#endif
            var solution = FastSolution(points);
            Console.WriteLine("{0:#0.0000}", solution);
        }

        private static double FastSolution(List<Point> points)
        {
            Debug.Assert(points.Count > 2, "points.Count > 2");
            points.Sort();

            var squaredDist = SplitAndMeasure(points, 0, points.Count - 1);
            return Math.Sqrt(squaredDist);
        }

        private static double SplitAndMeasure(List<Point> points, int startIndex, int endIndex)
        {
            var count = endIndex - startIndex + 1;
            Debug.Assert(count >= 2, "count >= 2");

            if (count == 2)
            {
                return points[startIndex].DistanceSq(points[endIndex]);
            }

            if (count == 3)
            {
                var a = points[startIndex];
                var b = points[startIndex + 1];
                var c = points[endIndex];

                var dab = a.DistanceSq(ref b);
                var dac = a.DistanceSq(ref c);
                var dbc = b.DistanceSq(ref c);

                return Math.Min(dab, Math.Min(dac, dbc));
            }

            var medianIdx = (startIndex + endIndex) / 2;
            var leftDistance = SplitAndMeasure(points, startIndex, medianIdx);
            var rightDistance = SplitAndMeasure(points, medianIdx + 1, endIndex);
            var distance = Math.Min(leftDistance, rightDistance);

            return distance;
        }
        
        private static Point P(int x, int y) => new Point(x, y);
        
        [DebuggerDisplay("({X}, {Y})")]
        private struct Point : IEquatable<Point>, IComparable<Point>
        {
            public readonly int X;
            public readonly int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool Equals(Point other)
            {
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Point && Equals((Point) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }

            public double DistanceSq(Point point) => DistanceSq(ref point);
            
            public double DistanceSq(ref Point point)
            {
                var dx = X - point.X;
                var dy = Y - point.Y;
                return dx * dx + dy * dy;
            }

            public int CompareTo(Point other)
            {
                var xComparison = X.CompareTo(other.X);
                if (xComparison != 0) return xComparison;
                return Y.CompareTo(other.Y);
            }
        }

        private static void ParseInputs(out List<Point> points)
        {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input);

            points = new List<Point>(n);
            for (var i = 0; i < n; ++i)
            {
                input = Console.ReadLine();
                var coordinates = input.Split();
                var x = int.Parse(coordinates[0]);
                var y = int.Parse(coordinates[1]);
                points.Add(new Point(x, y));
            }
        }
    }
}