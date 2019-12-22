using System;
using System.Collections;
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

        public static long Solution(int maxWeight, List<int> itemWeights)
        {
            return 0;
        }

        private static void RunMain()
        {
            int maxWeight;
            var itemWeights = ParseInputs(out maxWeight);

            var solution = Solution(maxWeight, itemWeights);
            Console.WriteLine(solution);
        }

        private static List<int> ParseInputs(out int maxWeight)
        {
            var expectedCount = 0;

            {
                var line = Console.ReadLine();
                var values = line.Split().Select(int.Parse).ToList();
                Debug.Assert(values.Count == 2, "values.Count == 2");
                maxWeight = values[0];
                expectedCount = values[1];
            }

            {
                var line = Console.ReadLine();
                var weights = line.Split().Select(int.Parse).ToList();
                Debug.Assert(weights.Count == expectedCount, "weights.Count == expectedCount");
                return weights;
            }
        }
    }
}