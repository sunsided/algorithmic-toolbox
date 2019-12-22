using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week5.LongestCommonSubsequenceOfThree
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            Testing.Run();
#else
            RunMain();
#endif
        }

        public static bool Solution(List<int> values)
        {
            // Since the goal is to evenly split across three people, we need at least three items.
            if (values.Count < 3) return false;

            // In order to evenly split, the sum of values must be integer divisible by the number of people.
            var sumOfItems = values.Sum();
            if (sumOfItems % 3 != 0) return false;

            return true;
        }

        private static void RunMain()
        {
            List<int> values;
            ParseInputs(out values);

            var solution = Solution(values);
            Console.WriteLine(solution ? "1" : "0");
        }

        private static void ParseInputs(out List<int> values)
        {
            var expectedCount = int.Parse(Console.ReadLine());
            values = Console.ReadLine().Split().Select(int.Parse).ToList();
            Debug.Assert(values.Count == expectedCount, "weights.Count == expectedCount");
        }
    }
}