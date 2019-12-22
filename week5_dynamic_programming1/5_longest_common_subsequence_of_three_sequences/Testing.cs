using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Week5.LongestCommonSubsequenceOfThree
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
                var length = Program.Solution(testCase.First, testCase.Second, testCase.Third, ref lcs);
                Debug.Assert(lcs.Count == length, "lcs.Count == length");
                Debug.Assert(length == testCase.Length, "length == testCase.Length");

                if (lcs.Count == 0) continue;
                Debug.Assert(expectedSequences.Any(s => s.SequenceEqual(lcs)), "expectedSequences.Any(s => s.SequenceEqual(lcs))");
            }
        }

        private static IEnumerable<Test> GenerateTestCases()
        {
            yield return GenerateTestCase(
                In(1, 2, 3),
                In(2, 1, 3),
                In(1, 3, 5),
                Out(1, 3));

            yield return GenerateTestCase(
                In(8, 3, 2, 1, 7),
                In(8, 2, 1, 3, 8, 10, 7),
                In(6, 8, 3, 1, 4, 7),
                Out(8, 3, 7), Out(8, 1, 7));
        }

        private static Test GenerateTestCase(InputSequence first, InputSequence second, InputSequence third, params OutputSequence[] expectedSequences)
        {
            var length = expectedSequences.Length > 0 ? expectedSequences[0].Count : 0;
            Debug.Assert(expectedSequences.All(s => s.Count == length), "expectedSequences.All(s => s.Count == length)");
            return GenerateTestCase(length, first, second, third, expectedSequences);
        }

        private static Test GenerateTestCase(int length, InputSequence first, InputSequence second, InputSequence third, params OutputSequence[] expectedSequences)
        {
            return new Test(length, first, second, third, expectedSequences.ToList());
        }

        private static InputSequence In(params long[] values)
        {
            return new InputSequence(values);
        }

        private static OutputSequence Out(params long[] values)
        {
            return new OutputSequence(values);
        }

        private struct Test
        {
            public readonly int Length;
            public readonly InputSequence First;
            public readonly InputSequence Second;
            public readonly InputSequence Third;
            public readonly Collection<OutputSequence> CommonSubsequences;

            public Test(int length, InputSequence first, InputSequence second, InputSequence third, IList<OutputSequence> commonSubsequences)
            {
                Length = length;
                First = first;
                Second = second;
                Third = third;
                CommonSubsequences = new Collection<OutputSequence>(commonSubsequences);
            }
        }

        private sealed class OutputSequence : List<long>
        {
            public OutputSequence(IEnumerable<long> collection) : base(collection) { }
        }
    }
}