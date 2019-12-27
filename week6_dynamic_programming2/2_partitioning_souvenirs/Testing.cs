using System.Collections.Generic;
using System.Diagnostics;

namespace Week6.LongestCommonSubsequenceOfThree
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            foreach (var testCase in GenerateTestCases())
            {
                var outcome = Program.Solution(testCase.ItemValues);
                Debug.Assert(outcome == testCase.ExpectedOutcome,
                    $"Result {outcome} != {testCase.ExpectedOutcome} for {string.Join(" ", testCase.ItemValues)}");
            }
        }

        private static IEnumerable<Test> GenerateTestCases()
        {
            // From a forum post.
            yield return GenerateTestCase(true, In(1, 1, 1));

            // Neither is 40 divisible by 3, nor is it enough items for three people (at least three).
            yield return GenerateTestCase(false, In(40));

            // The sum of the items is divisible by 3, but no three groups of identical value can be created.
            yield return GenerateTestCase(false, In(3, 3, 3, 3));

            // Groups are: (7, 3) = (5, 4, 1) = (8, 2), all of value 10.
            yield return GenerateTestCase(true, In(7, 2, 3, 1, 5, 4, 8));

            // Groups are: (7, 3) = (5, 4, 1) = (8, 2), all of value 10.
            yield return GenerateTestCase(true, In(7, 3, 2, 1, 5, 4, 8));

            // From a forum post.
            yield return GenerateTestCase(false, In(3, 3, 4, 5, 6));

            // Groups are: (2, 4) = (1, 5) = (6), all of value 6.
            yield return GenerateTestCase(true, In(2, 1, 4, 5, 6));

            // Not divisible by 3.
            yield return GenerateTestCase(false, In(2, 1, 5, 5, 6));

            // Groups are: (34 + 67 + 17) = (23 + 59 + 1 + 17 + 18) = (59 + 2 + 57), all of value 118.
            yield return GenerateTestCase(true, In(17, 59, 34, 57, 17, 23, 67, 1, 18, 2, 59));

            // Groups are: (1 + 3 + 7 + 25) = (2 + 4 + 5 + 7 + 8 + 10) = (5 + 12 + 19), all of value 36.
            yield return GenerateTestCase(true, In(1, 2, 3, 4, 5, 5, 7, 7, 8, 10, 12, 19, 25));
        }

        private static Test GenerateTestCase(bool expectedOutcome, List<int> itemValues)
        {
            return new Test(expectedOutcome, itemValues);
        }

        private static List<int> In(params int[] values)
        {
            return new List<int>(values);
        }

        private struct Test
        {
            public readonly bool ExpectedOutcome;
            public readonly List<int> ItemValues;

            public Test(bool expectedOutcome, List<int> itemValues)
            {
                ExpectedOutcome = expectedOutcome;
                ItemValues = itemValues;
            }
        }
    }
}