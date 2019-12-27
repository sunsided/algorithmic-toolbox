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

        public static bool Solution(List<int> itemWeights)
        {
            // Since the goal is to evenly split across three people, we need at least three items.
            if (itemWeights.Count < 3) return false;

            // In order to evenly split, the sum of values must be integer divisible by the number of people.
            var sumOfItems = itemWeights.Sum();
            if (sumOfItems % 3 != 0) return false;

            var maxWeight = sumOfItems / 3;
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
                        var newValueA = values[w - currentItemWeight, i - 1].ValueA + currentItemValue;
                        var newValueB = values[w - currentItemWeight, i - 1].ValueB + currentItemValue;

                        // values[w, i] = Math.Max(newValue, values[w, i]);
                        var previous = values[w, i];
                        if (previous.ValueA < newValueA)
                        {
                            values[w, i] = new Bipartition(currentItemValue, previous);
                        }
                        else if (previous.ValueB < newValueB)
                        {
                            values[w, i] = new Bipartition(previous, currentItemValue);
                        }
                    }

                    // Early exit
                    if (values[w, i].ValueA == maxWeight && values[w, i].ValueB == maxWeight) return true;
                }
            }

            // Since the sum is divisible by three, if we find two partitions of the same size,
            // the remaining one must be valid, too.
            var resultValue = values[maxWeight, itemWeights.Count];
            return resultValue.ValueA == maxWeight && resultValue.ValueB == maxWeight;
        }

        private static Bipartition[,] InitializeSelectionMatrix(int knapsackSize, ICollection weights)
        {
            // Note that we need the first row and column to be initialized with zero.
            // Intuitively, a knapsack with zero size has zero value, as has a knapsack with zero items.
            var list = new Bipartition[knapsackSize + 1, weights.Count + 1];
            for (var s = 0; s < knapsackSize + 1; ++s)
            {
                for (var w = 0; w < weights.Count + 1; ++w)
                {
                    list[s, w] = new Bipartition(
                        new List<int>(),
                        new List<int>());
                }
            }

            return list;
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

        [DebuggerDisplay("({ValueA}, {ValueB})")]
        private struct Bipartition
        {
            public Bipartition(List<int> listA, List<int> listB)
            {
                ListA = new List<int>(listA);
                ListB = new List<int>(listB);
            }

            public Bipartition(int valueA, Bipartition previous)
            {
                ListA = new List<int>(previous.ListA) {valueA};
                ListB = new List<int>(previous.ListB);
            }

            public Bipartition(Bipartition previous, int valueB)
            {
                ListA = new List<int>(previous.ListA);
                ListB = new List<int>(previous.ListB) {valueB};
            }

            public int ValueA => ListA.Sum();
            public int ValueB => ListB.Sum();

            public List<int> ListA { get; }
            public List<int> ListB { get; }
        }
    }
}