using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Week5.LongestCommonSubsequence
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            foreach (var testCase in GenerateTestCases())
            {
                var expectedSequences = testCase.CommonSubsequences;

                var lcs = new List<long>();
                var length = Program.Solution(testCase.First, testCase.Second, ref lcs);

                Debug.Assert(lcs.Count == length, "lcs.Count == length");
                Debug.Assert(length == testCase.Length, "length == testCase.Length");

                if (lcs.Count == 0) continue;
                Debug.Assert(expectedSequences.Any(s => s.SequenceEqual(lcs)), "expectedSequences.Any(s => s.SequenceEqual(lcs))");
            }
        }

        private static IEnumerable<Test> GenerateTestCases()
        {
            // This test has the single solution (2, 5).
            yield return GenerateTestCase(2, S(2, 7, 5), S(2, 5),
                S(2, 5));

            // This test has no shared elements.
            yield return GenerateTestCase(0, S(7), S(1, 2, 3, 4));

            // This test has exactly one shared element, (4).
            yield return GenerateTestCase(1, S(4), S(1, 2, 3, 4),
                S(4));

            // This test has two solutions (2, 7) and (2, 8).
            yield return GenerateTestCase(2, S(2, 7, 8, 3), S(5, 2, 8, 7),
                S(2, 7), S(2, 8));
        }

        private static Test GenerateTestCase(int length, List<long> first, List<long> second, params List<long>[] expectedSequences)
        {
            return new Test(length, first, second, expectedSequences.ToList());
        }

        private static List<long> S(params long[] values)
        {
            return new List<long>(values);
        }

        private struct Test
        {
            public readonly int Length;
            public readonly List<long> First;
            public readonly List<long> Second;
            public readonly Collection<List<long>> CommonSubsequences;

            public Test(int length, List<long> first, List<long> second, IList<List<long>> commonSubsequences)
            {
                Length = length;
                First = first;
                Second = second;
                CommonSubsequences = new Collection<List<long>>(commonSubsequences);
            }
        }
    }
}