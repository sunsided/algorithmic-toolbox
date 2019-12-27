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
            var values = InitializeSelectionMatrix(maxWeight, itemWeights);

            for (var i = 1; i <= itemWeights.Count; ++i)
            {
                // Since we're talking about gold bars, value/weight ratio is 1.
                var currentItemWeight = itemWeights[i - 1];
                var currentItemValue = currentItemWeight;

                for (var w = 1; w <= maxWeight; ++w)
                {
                    // Initialize the current solution with the previous best value.
                    values[w, i] = values[w, i - 1];

                    // Check whether we can improve on the value.
                    if (currentItemWeight <= w)
                    {
                        var newValue = values[w - currentItemWeight, i - 1] + currentItemValue;
                        values[w, i] = Math.Max(newValue, values[w, i]);
                    }
                }
            }

            // TODO: Backtrack by seeing whether having _added_ the item improved - or not having it at all kept - the value.
            return values[maxWeight, itemWeights.Count];
        }

        private static int[,] InitializeSelectionMatrix(int knapsackSize, ICollection weights)
        {
            // Note that we need the first row and column to be initialized with zero.
            // Intuitively, a knapsack with zero size has zero value, as has a knapsack with zero items.
            return new int[knapsackSize + 1, weights.Count + 1];
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