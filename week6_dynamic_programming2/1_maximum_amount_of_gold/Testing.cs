using System.Collections.Generic;
using System.Diagnostics;

namespace Week5.LongestCommonSubsequenceOfThree
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            foreach (var testCase in GenerateTestCases())
            {
                var maximumValue = Program.Solution(testCase.MaximumWeight, testCase.ItemWeights);
                Debug.Assert(maximumValue == testCase.ExpectedValue, "maximumValue == testCase.ExpectedResult");
            }
        }

        private static IEnumerable<Test> GenerateTestCases()
        {
            // The solution here is (1, 8), which is the only combination whose sum is smaller than 10.
            yield return GenerateTestCase(9, 10, In(1, 4, 8));

            // Automated test case #3/14
            yield return GenerateTestCase(10, 10, In(3, 5, 3, 3, 5));
        }

        private static Test GenerateTestCase(int expectedValue, int maximumWeight, List<int> itemWeights)
        {
            return new Test(expectedValue, maximumWeight, itemWeights);
        }

        private static List<int> In(params int[] values)
        {
            return new List<int>(values);
        }

        private struct Test
        {
            public readonly int ExpectedValue;
            public readonly int MaximumWeight;
            public readonly List<int> ItemWeights;

            public Test(int expectedValue, int maximumWeight, List<int> itemWeights)
            {
                ExpectedValue = expectedValue;
                MaximumWeight = maximumWeight;
                ItemWeights = itemWeights;
            }
        }
    }
}