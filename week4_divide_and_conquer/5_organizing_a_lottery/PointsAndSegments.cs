using System;
using System.Collections.Generic;
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

#if TESTING
            var segments = new List<LineSegment>
            {
                S(0, 5),
                S(-3, 2),
                S(7, 10)
            };
            var points = new[] {1, 6, 11};
#else
            int[] points;
            List<LineSegment> segments;
            ParseInputs(out segments, out points);
#endif

            var solution = FastSolution(segments, points);
#if TESTING
            var naiveSolution = NaiveSolution(segments, points);
            Debug.Assert(solution.Length == naiveSolution.Length, "solution.Length == naiveSolution.Length");
            Debug.Assert(solution.Zip(naiveSolution, (a, b) => a == b).All(x => x), "solution.Zip(naiveSolution, (a, b) => a == b).All(x => x)");
#endif
            
            Console.WriteLine(string.Join(" ", solution));
        }

        private static int[] NaiveSolution(List<LineSegment> segments, int[] points)
        {
            var cnt = new int[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                for (var j = 0; j < segments.Count; j++)
                {
                    if (segments[j].Contains(points[i]))
                    {
                        cnt[i]++;
                    }
                }
            }

            return cnt;
        }

        private static int[] FastSolution(List<LineSegment> segments, int[] points)
        {
            // Given that this is a lottery game where the number of points (selected
            // by the players) is much larger than the number of line segments (selected
            // by the lottery), it makes sense to spend time to sort the list of line
            // segments for fast querying.
            segments.Sort();

            // For quick boundary checking, we first grab the smallest and biggest values
            // in the list.
            var smallestNumber = segments.First().Start;
            var biggestNumber = segments.Last().End;
            
            // Sorting the query points themselves would help as well since we
            // could then either re-use previous results if they coincide or hope
            // for better cache locality since exploration of similar branches of the segment
            // memory would be more likely. However, since we need to return the
            // point data in the order they were inserted, this would require some
            // overhead for returning the data.
            
            var results = new int[points.Length];
            for (var pi = 0; pi < points.Length; ++pi)
            {
                var p = points[pi];
                if (p < smallestNumber || p > biggestNumber)
                {
                    results[pi] = 0;
                    continue;
                }
                
                results[pi] = BinarySearch(segments, p);
            }
            
            return results;
        }

        private static int BinarySearch<T>(T segments, int point)
            where T: IList<LineSegment>
        {
            var start = 0;
            var end = segments.Count - 1;

            while (start <= end)
            {
                var midpoint = (end + start) / 2;
                var token = segments[midpoint];
                
                // As soon as we hit a valid range, we're going to scan from there.
                if (token.Contains(point))
                {
                    var matches = 1;
                    
                    // Scan left to find all matching elements.
                    for (var i = midpoint - 1; i >= 0; --i)
                    {
                        if (!segments[i].Contains(point)) break;
                        ++matches;
                    }
                    
                    // Scan right to find all matching elements.
                    for (var i = midpoint + 1; i < segments.Count; ++i)
                    {
                        if (!segments[i].Contains(point)) break;
                        ++matches;
                    }

                    return matches;
                }
                
                // Termination criterion
                if (start == end) break;
                
                // If the point is to either side of the token element,
                // iterate there.
                if (point < token.Start)
                {
                    end = midpoint - 1;
                }
                else
                {
                    Debug.Assert(point > token.End, "point > token.End");
                    start = midpoint + 1;
                }
            }

            return 0;
        }
        
        private static LineSegment S(int start, int end) => new LineSegment(start, end);

        [DebuggerDisplay("{Start} .. {End}")]
        private struct LineSegment : IComparable<LineSegment>, IEquatable<LineSegment>
        {
            public readonly int Start;
            public readonly int End;

            public LineSegment(int start, int end)
            {
                Debug.Assert(start <= end, "start <= end");
                Start = start;
                End = end;
            }
            
            public bool Contains(int point) => Start <= point && point <= End;

            #region Comparison and equatability
            
            public static bool operator ==(LineSegment a, LineSegment b) => a.CompareTo(b) == 0;
            public static bool operator !=(LineSegment a, LineSegment b) => a.CompareTo(b) != 0;
            public static bool operator <(LineSegment a, LineSegment b) => a.CompareTo(b) < 0;
            public static bool operator >(LineSegment a, LineSegment b) => a.CompareTo(b) > 0;
            public static bool operator <=(LineSegment a, LineSegment b) => a.CompareTo(b) <= 0;
            public static bool operator >=(LineSegment a, LineSegment b) => a.CompareTo(b) >= 0;
            
            public bool Equals(LineSegment other) => CompareTo(other) == 0;

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is LineSegment && Equals((LineSegment) obj);
            }

            public int CompareTo(LineSegment other)
            {
                var startComparison = Start.CompareTo(other.Start);
                if (startComparison != 0) return startComparison;
                return End.CompareTo(other.End);
            }
            
            #endregion Comparison and equatability
            
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Start * 397) ^ End;
                }
            }
        }
        
        private static void ParseInputs(out List<LineSegment> segments, out int[] points)
        {
            // Read segment and point count
            var sp = ReadIntegers(2);
            var s = sp[0];
            var p = sp[1];

            // Read all the line segments
            segments = new List<LineSegment>(s);
            for (var i = 0; i < s; ++i)
            {
                var segment = ReadIntegers(2);
                segments.Add(new LineSegment(segment[0], segment[1]));
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