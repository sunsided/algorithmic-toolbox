using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClosestPoints
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            if (!TestA()) return;
            RunTests();
#else
            List<Point> points;
            ParseInputs(out points);
            var solution = FastSolution(points);
            Console.WriteLine("{0:#0.0000}", solution);
#endif
        }

        [Conditional("DEBUG")]
        private static void RunTests()
        {
            var seedGenerator = new Random();
            var seed = seedGenerator.Next();

            var testIndex = -1;
            foreach (var testCase in TestCases)
            {
                ++testIndex;
                for (var retry = 0; retry < 5; ++retry)
                {
                    var shouldShuffle = retry > 0;

                    // After the first round, shuffle the list.
                    var inputData = testCase.Points.ToList();
                    if (shouldShuffle) Shuffle(inputData, ++seed);

                    // We make a copy of the data here, as the solution is going to re-order the list internally.
                    var testData = inputData.ToList();

                    // Run the solution
                    var fastSolution = double.NaN;
                    try
                    {
                        fastSolution = FastSolution(testData);
                        if (fastSolution == testCase.ExpectedDistance) continue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    PrintTestError(testCase, inputData, fastSolution);
                    return;
                }
            }

            Console.WriteLine("All tests OK.");
        }

        private static bool TestA()
        {
            var testCase = BuildTestCase(
                P(1716019236, 6),
                P(2008853278, 1985140336),
                P(1, 1),
                P(1501650843, 1)
            );

            return RunTestAndReportOnFailure(testCase);
        }

        private static bool RunTestAndReportOnFailure(TestCase testCase)
        {
            var distance = double.NaN;
            try
            {
                distance = FastSolution(testCase.Points.ToList());
                if (distance == testCase.ExpectedDistance)
                {
                    Console.WriteLine("Test OK");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            PrintTestError(testCase, testCase.Points, distance);
            return false;
        }

        private static void PrintTestError(TestCase testCase, List<Point> inputData, double solution)
        {
            Console.WriteLine("Expected distance {0:e}, but got {1:e}", testCase.ExpectedDistance, solution);
            Console.WriteLine("Data:");
            foreach (var point in inputData)
            {
                Console.WriteLine("{0},", point);
            }
        }

        private static double SlowSolution(List<Point> points)
        {
            Debug.Assert(points.Count >= 2, "points.Count >= 2");
            var distanceSq = double.MaxValue;
            for (var x = 0; x < points.Count - 1; ++x)
            {
                var pt = points[x];
                for (var y = x + 1; y < points.Count; ++y)
                {
                    var localDistance = points[y].DistanceSq(ref pt);
                    if (localDistance < 0 || (localDistance == 0 && !points[x].Equals(points[y]))) Debugger.Break();
                    Debug.Assert(localDistance >= 0,
                        string.Format("{0}.DistanceSq({1}) == {2}", points[0], points[1], localDistance));

                    distanceSq = Math.Min(distanceSq, localDistance);
                }
            }

            return Math.Sqrt(distanceSq);
        }

        private static double FastSolution(List<Point> points)
        {
            Debug.Assert(points.Count >= 2, "points.Count >= 2");
            var squaredDist = SplitAndMeasureSq(points, 0, points.Count - 1);
            return Math.Sqrt(squaredDist);
        }

        private static double SplitAndMeasureSq(List<Point> points, int startIndex, int endIndex, int depth = 0)
        {
            // TODO: To debug, wrap this in a checked{} block!

            // First, we apply a sort to order the points according to their X coordinate.
            points.Sort(startIndex, endIndex - startIndex + 1, PointXComparer.Default);

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

                Debug.Assert(!double.IsNaN(dab), "!double.IsNaN(dab)");
                Debug.Assert(!double.IsNaN(dac), "!double.IsNaN(dac)");
                Debug.Assert(!double.IsNaN(dbc), "!double.IsNaN(dbc)");

                return Math.Min(dab, Math.Min(dac, dbc));
            }

            var medianIdx = (startIndex + endIndex) / 2;
            var leftDistance = SplitAndMeasureSq(points, startIndex, medianIdx, depth + 1);
            var rightDistance = SplitAndMeasureSq(points, medianIdx + 1, endIndex, depth + 1);
            var distanceSq = Math.Min(leftDistance, rightDistance);
            if (distanceSq <= 0)
            {
                return 0;
            }

            // Next, we need to restore the sorting.
            points.Sort(startIndex, endIndex - startIndex + 1, PointXComparer.Default);

            // Now, we're going to select all items that are less than or equal to distance
            // units away from the split, in both of the splits.
            var medianX = points[medianIdx].X;
            var distance = Math.Sqrt(distanceSq);
            var lowerBound = medianX - distance;
            var upperBound = medianX + distance;

            // It is crucial to cast to _long_ here, as a cast to _int_ may underflow.
            var leftSplit = points.BinarySearch(new Point((long) Math.Round(lowerBound, MidpointRounding.AwayFromZero), 0), PointXComparer.Default);
            // Ensure we capture all the relevant points to the left.
            if (leftSplit < 0)
            {
                leftSplit = ~leftSplit;
            }
            else
            {
                while (leftSplit > 0 && points[leftSplit - 1].X >= lowerBound)
                {
                    --leftSplit;
                }
            }

            // It is crucial to cast to _long_ here, as a cast to _int_ may overflow.
            var rightSplit = points.BinarySearch(new Point((long) Math.Round(upperBound, MidpointRounding.AwayFromZero), 0), PointXComparer.Default);
            // Ensure we capture all the relevant points to the right.
            if (rightSplit < 0)
            {
                // We increase one here in order to land "on" the item
                rightSplit = ~rightSplit;
            }
            else
            {
                while (rightSplit < points.Count - 1 && points[rightSplit + 1].X <= upperBound)
                {
                    ++rightSplit;
                }
            }

            // Clamp the splits to their boundaries
            leftSplit = Math.Max(startIndex, Math.Min(endIndex, leftSplit));
            rightSplit = Math.Max(startIndex, Math.Min(endIndex, rightSplit));

            if (leftSplit > rightSplit) throw new InvalidOperationException(string.Format("Asserted: leftSplit <= rightSplit | {0} < {1}", leftSplit, rightSplit));
            var centerSplitCount = rightSplit - leftSplit;

            if (centerSplitCount < 0) throw new InvalidOperationException(string.Format("Asserted: centerSplitCount >= 0 | {0}", centerSplitCount));
            if (leftSplit + centerSplitCount >= points.Count) throw new InvalidOperationException(string.Format("Asserted: leftSplit + centerSplitCount < points.Count | {0} + {1} < {2}", leftSplit, centerSplitCount, points.Count));

            // Sort the new segment.
            points.Sort(leftSplit, centerSplitCount, PointYComparer.Default);

            // Compare each remaining point to its seven neighbors.
            for (var i = leftSplit; i < rightSplit; ++i)
            {
                for (var j = i + 1; j < Math.Min(i + 7, rightSplit + 1); ++j)
                {
                    var pointDistSq = points[i].DistanceSq(points[j]);
                    if (pointDistSq < distanceSq)
                    {
                        distanceSq = pointDistSq;
                    }
                }
            }

            return distanceSq;
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

        private static void Shuffle<T>(List<T> array, int seed)
        {
            var random = new Random(seed);
            for (var i = array.Count; i > 1; --i)
            {
                var j = random.Next(i);
                var tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        private static IEnumerable<TestCase> TestCases
        {
            get
            {
                #region Smoke Tests: Default Test Cases

                yield return new TestCase(5.0,
                    new List<Point>
                    {
                        P(0, 0),
                        P(3, 4),
                    });

                yield return new TestCase(0.0,
                    new List<Point>
                    {
                        P(7, 7),
                        P(1, 100),
                        P(4, 8),
                        P(7, 7)
                    });

                yield return new TestCase(Math.Sqrt(2),
                    new List<Point>
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
                    });

                #endregion Smoke Tests: Default Test Cases

                #region Additional debugging cases

                yield return new TestCase(0,
                    new List<Point>
                    {
                        P(7, 7),
                        P(1, 100),
                        P(-2, 100),
                        P(4, 8),

                        P(7, 7),
                        P(8, 6),
                        P(996, -100),
                        P(1000, -100),
                    });

                yield return new TestCase(0,
                    new List<Point>
                    {
                        P(7, 7),
                        P(1, 100),
                        P(-2, 100),
                        P(4, 8),

                        P(7, 7),
                        P(8, 6),
                        P(996, -100),
                    });

                yield return new TestCase(0,
                    new List<Point>
                    {
                        P(7, 7),
                        P(1, 100),
                        P(4, 8),

                        P(7, 7),
                        P(8, 6),
                        P(996, -100),
                        P(1000, -100),
                    });

                yield return new TestCase(1.0,
                    new List<Point>
                    {
                        P(0, 0),
                        P(1, 0),
                        P(0, 1),
                    });

                #endregion Additional debugging cases

                #region Test cases known to fail

                // Wrong result
                yield return BuildTestCase(
                    P(1716019236, 6),
                    P(2008853278, 1985140336),
                    P(1, 1),
                    P(1501650843, 1)
                );

                // Exception
                yield return BuildTestCase(
                    P(1743536468, 258964953),
                    P(736238120, 893362053),
                    P(1768871663, 1909821817),
                    P(813541575, 323035883),
                    P(1576743814, 434067991)
                );

                // Exception
                yield return BuildTestCase(
                    P(1922919673, 76983070),
                    P(308882645, 1505345705),
                    P(2084262107, 1503548551),
                    P(1047327236, 1723379800),
                    P(2137082901, 1181371557)
                );

                yield return BuildTestCase(
                    P(352247606, 637066663),
                        P(479876756, 99421972),
                        P(738143135, 839794419),
                        P(1273023503, 1778884288),
                        P(1470125282, 221910935),
                        P(1532039334, 74070668)
                    );

                yield return BuildTestCase(
                    P(407722186, 268780282),
                    P(886914344, 976331384),
                    P(963968123, 12240710),
                    P(1136531059, 1269754387),
                    P(1239061428, 1320294707),
                    P(1442329418, 1787046623),
                    P(1681668034, 681214485),
                    P(1728612833, 379202077));

                #endregion Test cases known to fail

                #region Generated test cases

                var rand = new Random();

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(0, 100), rand.Next(0, 100))).ToList();
                    yield return BuildTestCase(points);
                }

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(-100, 0), rand.Next(-100, 0))).ToList();
                    yield return BuildTestCase(points);
                }

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(-100, 0), rand.Next(0, 100))).ToList();
                    yield return BuildTestCase(points);
                }

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(0, 100), rand.Next(-100, 0))).ToList();
                    yield return BuildTestCase(points);
                }

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(-100, 100), rand.Next(-100, 100))).ToList();
                    yield return BuildTestCase(points);
                }

                for (var i = 0; i < 100; ++i)
                {
                    var count = rand.Next(2, 10);
                    var points = Enumerable.Range(0, count).Select(_ => P(rand.Next(), rand.Next())).ToList();
                    yield return BuildTestCase(points);
                }

                #endregion Generated test cases
            }
        }

        private static TestCase BuildTestCase(params Point[] points)
        {
            return BuildTestCase(points.ToList());
        }

        private static TestCase BuildTestCase(List<Point> points)
        {
            var expectedValue = SlowSolution(points);
            if (double.IsNaN(expectedValue)) Debugger.Break();
            Debug.Assert(!double.IsNaN(expectedValue), "double.IsNaN(expectedValue)");

            return new TestCase(expectedValue, points);
        }

        private static Point P(int x, int y) => new Point(x, y);

        [DebuggerDisplay("({X}, {Y})")]
        private struct Point : IEquatable<Point>, IComparable<Point>
        {
            public readonly long X;
            public readonly long Y;

            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return string.Format("P({0}, {1})", X, Y);
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
                    return (int)((X * 397) ^ Y);
                }
            }

            public double DistanceSq(Point point) => DistanceSq(ref point);

            public double DistanceSq(ref Point point)
            {
                double dx = X - point.X;
                double dy = Y - point.Y;
                return dx * dx + dy * dy;
            }

            public int CompareTo(Point other)
            {
                var xComparison = X.CompareTo(other.X);
                if (xComparison != 0) return xComparison;
                return Y.CompareTo(other.Y);
            }
        }

        private sealed class PointXComparer : IComparer<Point>
        {
            public static readonly PointXComparer Default = new PointXComparer();

            public int Compare(Point x, Point y) => x.X.CompareTo(y.X);
        }

        private sealed class PointYComparer : IComparer<Point>
        {
            public static readonly PointYComparer Default = new PointYComparer();

            public int Compare(Point x, Point y) => x.Y.CompareTo(y.Y);
        }

        private struct TestCase
        {
            public TestCase(double expectedDistance, List<Point> points)
            {
                ExpectedDistance = expectedDistance;
                Points = points;
            }

            public double ExpectedDistance { get; }
            public List<Point> Points { get; }
        }
    }
}