using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Week5.MoneyChangeAgain
{
    internal static class Program
    {
        private static readonly int[] Denominations = { 1, 3, 4 };

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Array.Sort(Denominations);

#if TESTING
            Debug.Assert(Solution(2) == 2, "Sample 1");
            Debug.Assert(Solution(34) == 9, "Sample 2");
#else
            var amount = ParseInputs();
            var solution = Solution(amount);
            Console.WriteLine("{0}", solution);
#endif
        }

        private static int Solution(int amount)
        {
            if (amount == 0) return 0;

            var changes = new int[amount + 1];
            for (var currentAmount = 0; currentAmount <= amount; ++currentAmount)
            {
                changes[currentAmount] = MinNumCoins(currentAmount, changes);
            }

            return changes[amount];
        }

        private static int MinNumCoins<T>(int currentAmount, T changes)
            where T : IReadOnlyList<int>
        {
            var minChanges = int.MaxValue;
            for (var j = 0; j < Denominations.Length; ++j)
            {
                // If the remaining change is negative, this implies our currently selected
                // denomination is greater than the amount to change.
                // We assume that the denominations array is sorted, thus we can break out of the loop.
                var remainingChange = currentAmount - Denominations[j];
                if (remainingChange < 0) break;

                // We already have a solution for the remaining change, and since the currently
                // selected denomination fits
                var knownSolutionForRemainingChange = changes[remainingChange];
                var currentSolution = knownSolutionForRemainingChange + 1;

                minChanges = Math.Min(minChanges, currentSolution);
            }

            return minChanges < int.MaxValue ? minChanges : 0;
        }

        private static int ParseInputs()
        {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            return int.Parse(input);
        }
    }
}