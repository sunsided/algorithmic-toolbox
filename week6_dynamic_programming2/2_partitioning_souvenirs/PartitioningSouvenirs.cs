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

        public static bool Solution(List<int> itemWeights)
        {
            // Since the goal is to evenly split across three people, we need at least three items.
            if (itemWeights.Count < 3) return false;

            // In order to evenly split, the sum of values must be integer divisible by the number of people.
            var sumOfItems = itemWeights.Sum();
            if (sumOfItems % 3 != 0) return false;
            var partitionSize = sumOfItems / 3;

            var lookup = new Dictionary<Tuple<int, int, int, int>, bool>();

            return SubsetSumExists(itemWeights, itemWeights.Count - 1,
                partitionSize,
                partitionSize,
                partitionSize,
                lookup);
        }

        // Implemented from recursive solution at https://www.techiedelight.com/3-partition-problem/
        public static bool SubsetSumExists(List<int> weights, int n,
            int partitionSizeA,
            int partitionSizeB,
            int partitionSizeC,
            Dictionary<Tuple<int, int, int, int>, bool> lookup)
        {
            var lookupKey = Tuple.Create(partitionSizeA, partitionSizeB, partitionSizeC, n);
            bool solution;
            if (lookup.TryGetValue(lookupKey, out solution))
            {
                return solution;
            }

            // return true if subset is found
            if (partitionSizeA == 0 && partitionSizeB == 0 && partitionSizeC == 0)
            {
                return true;
            }

            // base case: no items left
            if (n < 0)
            {
                return false;
            }

            // Case 1. current item becomes part of first subset
            var includedInA = false;
            if (partitionSizeA - weights[n] >= 0)
            {
                includedInA = SubsetSumExists(weights, n - 1,
                    partitionSizeA - weights[n],
                    partitionSizeB,
                    partitionSizeC,
                    lookup);
            }

            // Case 2. current item becomes part of second subset
            var includedInB = false;
            if (!includedInA && partitionSizeB - weights[n] >= 0)
            {
                includedInB = SubsetSumExists(weights, n - 1,
                    partitionSizeA,
                    partitionSizeB - weights[n],
                    partitionSizeC,
                    lookup);
            }

            // Case 3. current item becomes part of third subset
            var includedInC = false;
            if (!includedInA && !includedInB && partitionSizeC - weights[n] >= 0)
            {
                includedInC = SubsetSumExists(weights, n - 1,
                    partitionSizeA,
                    partitionSizeB,
                    partitionSizeC - weights[n],
                    lookup);
            }

            // return true if we get solution
            solution = includedInA || includedInB || includedInC;
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