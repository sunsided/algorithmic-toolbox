using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PointsAndSegments
{
    internal static class Program
    {
#if TESTING
        public static readonly Random Random = new Random(0);
#endif
        
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            // Note that according to the problem statement,
            // the points aren't necessarily in monotonically non-decreasing order.

#if TESTING
            /*
            var segments = new List<LineSegment>
            {
                S(0, 5),
                S(-3, 2),
                S(7, 10)
            };
            var points = new[] {1, 6, 11};
            */

            const int min = 0; // -100000000;
            const int max = 1000; // 100000000;
            const int s = 1000; // 10000;
            const int p = 100000; // 100000;
            
            var starts = new List<int>(s);
            var ends = new List<int>(s);
            for (var i = 0; i < s; ++i)
            {
                GenerateSegment(min, max, starts, ends);
            }
            
            var points = Enumerable.Range(0, p).Select(x => GeneratePoint(min, max)).ToArray();
#else
            int[] points;
            List<int> starts;
            List<int> ends;
            ParseInputs(out starts, out ends, out points);
#endif

#if TESTING
            Console.WriteLine("Running slow solution ...");
            
            var naiveWatch = Stopwatch.StartNew();
            var naiveSolution = NaiveSolution(starts, ends, points);
            Console.WriteLine("Slow solution took {0}", naiveWatch.Elapsed);
            
            Console.WriteLine("Running fast solution ...");
#endif
            
            var stopwatch = Stopwatch.StartNew(); 
            var solution = FastSolution(starts, ends, points);
            
#if TESTING
            Console.WriteLine("Fast solution took {0}", stopwatch.Elapsed);
            Debug.Assert(solution.Length == naiveSolution.Length, "solution.Length == naiveSolution.Length");
            for (var i = 0; i < solution.Length; ++i)
            {
                if (solution[i] != naiveSolution[i])
                {
                    Console.Error.WriteLine("Mismatch at index {0} (value {3}): expected {1}, but got {2}", i, naiveSolution[i], solution[i], points[i]);
                }
            }
#else
            Console.WriteLine(string.Join(" ", solution));
#endif
        }

#if TESTING

        private static void GenerateSegment(int min, int max, List<int> starts, List<int> ends)
        {
            var start = Random.Next(min, max + 1);
            var end = Random.Next(start, max + 1);
            starts.Add(start);
            ends.Add(end);
        }
        
        private static int GeneratePoint(int min, int max) => Random.Next(min, max + 1);

#endif
        
        private static int[] NaiveSolution(List<int> starts, List<int> ends, int[] points)
        {
            var cnt = new int[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var pt = points[i];
                for (var j = 0; j < starts.Count; j++)
                {
                    if (starts[j] <= pt && pt <= ends[j])
                    {
                        cnt[i]++;
                    }
                }
            }

            return cnt;
        }

        private static int[] FastSolution(List<int> starts, List<int> ends, int[] points)
        {
            var deltas = BuildDeltaList(starts, ends);
            if (deltas.Count == 0) return new int[0];

            var startTime = deltas[0].Time;
            var endTime = deltas[deltas.Count - 1].Time;
            
            var results = new int[points.Length];
            for (var pi = 0; pi < points.Length; ++pi)
            {
                var pt = points[pi];
                if (pt < startTime || pt > endTime) continue;
                
                var idx = deltas.BinarySearch(new DeltaPoint(pt, 0), PointTimeComparer.Default);
                if (idx >= 0)
                {
                    results[pi] = deltas[idx].Value;
                }
                else
                {
                    idx = ~idx;
                    if (idx > endTime) continue;
                    results[pi] = deltas[idx - 1].Value;
                }
            }
            
            return results;
        }

        private static List<DeltaPoint> BuildDeltaList(List<int> starts, List<int> ends)
        {
            Debug.Assert(starts.Count == ends.Count, "starts.Count == ends.Count");

            // This simple termination criterion ensures that at least one element exists.
            if (starts.Count == 0 && ends.Count == 0)
            {
                return new List<DeltaPoint>(0);
            }

            // We sort the lists in reverse order so that removing from the list is fast.
            starts.Sort(ReverseComparer.Default);
            ends.Sort(ReverseComparer.Default);

            // We initialize the delta list with a starting element:
            // Before the very first start point, the count is zero.
            var deltas = new List<DeltaPoint>
            {
                new DeltaPoint(starts[starts.Count - 1] - 1, 0)
            };

            var startsCount = starts.Count;
            var endsCount = ends.Count;
            do
            {
                var previousDelta = deltas[deltas.Count - 1];

                if (startsCount > 0 && endsCount > 0)
                {
                    // Pick the right values.
                    var deltaTime = -1;
                    var deltaValue = 0;
                    
                    // Since the end time is still ON the line segment,
                    // the value actually only decreases AFTER the end.
                    if (starts[startsCount - 1] <= (ends[endsCount - 1] + 1))
                    {
                        deltaTime = starts[startsCount - 1];
                        deltaValue = 1;
                        starts.RemoveAt(startsCount - 1);
                    }
                    else
                    {
                        // As discussed, tthe end time needs to be shifted by 1.
                        deltaTime = ends[endsCount - 1] + 1;
                        deltaValue = -1;
                        ends.RemoveAt(endsCount - 1);
                    }

                    // Fold the results.
                    if (deltaTime == previousDelta.Time)
                    {
                        deltas[deltas.Count - 1] += deltaValue;
                    }
                    else
                    {
                        deltas.Add(new DeltaPoint(deltaTime, previousDelta.Value + deltaValue));
                    }
                }

                // Update the test criteria.
                startsCount = starts.Count;
                endsCount = ends.Count;
            } while (startsCount > 0 && endsCount > 0);

            // Fill in the missing elements; it can't be start elements because they
            // must be smaller than the end elements.
            Debug.Assert(starts.Count == 0 && ends.Count >= 0, "starts.Count == 0 && ends.Count >= 0");
            while (ends.Count > 0)
            {
                // Like before, we need to take into account that the end of the line segment
                // is still ON the line segment.
                var deltaTime = ends[ends.Count - 1] + 1;
                var deltaValue = -1;
                ends.RemoveAt(ends.Count - 1);

                // Fold the results.
                var previousDelta = deltas[deltas.Count - 1];
                if (deltaTime == previousDelta.Time)
                {
                    deltas[deltas.Count - 1] += deltaValue;
                }
                else
                {
                    deltas.Add(new DeltaPoint(deltaTime, previousDelta.Value + deltaValue));
                }
            }

            return deltas;
        }
        
        [DebuggerDisplay("{Value} at {Time}")]
        private struct DeltaPoint
        {
            public readonly int Time;
            public readonly int Value;

            public DeltaPoint(int time, int value)
            {
                Time = time;
                Value = value;
            }
            
            public static DeltaPoint operator +(DeltaPoint lhs, int value) => new DeltaPoint(lhs.Time, lhs.Value + value);
            public static DeltaPoint operator -(DeltaPoint lhs, int value) => new DeltaPoint(lhs.Time, lhs.Value - value);
        }
        
        private sealed class PointTimeComparer : IComparer<DeltaPoint>
        {
            public static readonly PointTimeComparer Default = new PointTimeComparer();
            
            public int Compare(DeltaPoint x, DeltaPoint y) => x.Time.CompareTo(y.Time);
        }
        
        private sealed class ReverseComparer : IComparer<int>
        {
            public static readonly ReverseComparer Default = new ReverseComparer();

            public int Compare(int x, int y) => y.CompareTo(x);
        }
        
        private static void ParseInputs(out List<int> starts, out List<int> ends, out int[] points)
        {
            // Read segment and point count
            var sp = ReadIntegers(2);
            var s = sp[0];
            var p = sp[1];

            // Read all the line segments
            starts = new List<int>(s);
            ends = new List<int>(s);
            for (var i = 0; i < s; ++i)
            {
                var segment = ReadIntegers(2);
                starts.Add(segment[0]);
                ends.Add(segment[1]);
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