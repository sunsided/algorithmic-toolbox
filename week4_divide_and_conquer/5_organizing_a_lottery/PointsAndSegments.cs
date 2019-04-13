using System;
using System.Diagnostics;
using System.Linq;

namespace PointsAndSegments
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            // Note that according to the problem statement,
            // the points aren't necessarily in monotonically non-decreasing order.
            
            int[] points;
            LineSegment[] segments;

#if TESTING
            segments = new []
            {
                S(0, 5),
                S(-3, 2),
                S(7, 10)
            };
            points = new[] {1, 6};
#else
            ParseInputs(out segments, out points);
#endif

            var solution = FastSolution(segments, points);
#if TESTING
            var naiveSolution = NaiveSolution(segments, points);
            Debug.Assert(solution.Zip(naiveSolution, (a, b) => a == b).All(x => x), "solution.Zip(naiveSolution, (a, b) => a == b).All(x => x)");
#endif
            
            Console.WriteLine(string.Join(" ", solution));
        }

        private static int[] NaiveSolution(LineSegment[] segments, int[] points)
        {
            var cnt = new int[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                for (var j = 0; j < segments.Length; j++)
                {
                    if (segments[j].Contains(points[i]))
                    {
                        cnt[i]++;
                    }
                }
            }

            return cnt;
        }

        private static int[] FastSolution(LineSegment[] segments, int[] points)
        {
            return NaiveSolution(segments, points);
        }

        private static LineSegment S(int start, int end) => new LineSegment(start, end);

        [DebuggerDisplay("{Start} .. {End}")]
        private struct LineSegment
        {
            public readonly int Start;
            public readonly int End;

            public LineSegment(int start, int end)
            {
                Start = start;
                End = end;
            }
            
            public bool Contains(int point) => Start <= point && point <= End;
        }
        
        private static void ParseInputs(out LineSegment[] segments, out int[] points)
        {
            // Read segment and point count
            var sp = ReadIntegers(2);
            var s = sp[0];
            var p = sp[1];

            // Read all the line segments
            segments = new LineSegment[s];
            for (var i = 0; i < s; ++i)
            {
                var segment = ReadIntegers(2);
                segments[i] = new LineSegment(segment[0], segment[1]);
            }
            
            // Read all the points
            points = ReadIntegers(p);
        }

        private static int[] ReadIntegers(int n = 0)
        {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var inputs = input.Split();

            Debug.Assert(n <= 0 || n == inputs.Length, "n <= 0 || n == inputs.Length");
            
            var values = new int[inputs.Length];
            for (var i = 0; i < n; ++i)
            {
                values[i] = int.Parse(inputs[i]);
            }

            return values;
        }
    }
}