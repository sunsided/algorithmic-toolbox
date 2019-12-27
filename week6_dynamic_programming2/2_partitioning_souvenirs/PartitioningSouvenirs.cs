using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PartitionSizes = System.Tuple<int, int, int>;

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

        public static bool Solution(List<int> itemWeights)
        {
            // Since the goal is to evenly split across three people, we need at least three items.
            if (itemWeights.Count < 3) return false;

            // In order to evenly split, the sum of values must be integer divisible by the number of people.
            var sumOfItems = itemWeights.Sum();
            if (sumOfItems % 3 != 0) return false;
            var partitionSize = sumOfItems / 3;

            var lookup = new Dictionary<Tuple<PartitionSizes, int>, bool>();

            return SubsetSumExists(itemWeights, itemWeights.Count - 1,
                Tuple.Create(partitionSize, partitionSize, partitionSize),
                lookup);
        }

        // Implemented from recursive solution at https://www.techiedelight.com/3-partition-problem/
        public static bool SubsetSumExists(List<int> weights, int n,
            Tuple<int, int, int> partitionSizes,
            Dictionary<Tuple<PartitionSizes, int>, bool> lookup)
        {
            var lookupKey = Tuple.Create(partitionSizes, n);
            bool solution;
            if (lookup.TryGetValue(lookupKey, out solution))
            {
                return solution;
            }

            // return true if subset is found
            if (partitionSizes.Item1 == 0 && partitionSizes.Item2 == 0 && partitionSizes.Item3 == 0)
            {
                return true;
            }

            // base case: no items left
            if (n < 0)
            {
                return false;
            }

            // Case 1. current item becomes part of first subset
            if (partitionSizes.Item1 - weights[n] >= 0)
            {
                solution = SubsetSumExists(weights, n - 1,
                    Tuple.Create(
                        partitionSizes.Item1 - weights[n],
                        partitionSizes.Item2,
                        partitionSizes.Item3),
                lookup);
            }

            // Case 2. current item becomes part of second subset
            if (!solution && partitionSizes.Item2 - weights[n] >= 0)
            {
                solution = SubsetSumExists(weights, n - 1,
                    Tuple.Create(
                        partitionSizes.Item1,
                        partitionSizes.Item2 - weights[n],
                        partitionSizes.Item3),
                    lookup);
            }

            // Case 3. current item becomes part of third subset
            if (!solution && partitionSizes.Item3 - weights[n] >= 0)
            {
                solution = SubsetSumExists(weights, n - 1,
                    Tuple.Create(
                        partitionSizes.Item1,
                        partitionSizes.Item2,
                        partitionSizes.Item3 - weights[n]),
                    lookup);
            }

            lookup.Add(lookupKey, solution);
            return solution;
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