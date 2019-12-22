using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChangeDP
{
    internal static class Program
    {
        private static readonly Func<int, int>[] Operations =
        {
            x => x + 1,
            x => 2 * x,
            x => 3 * x
        };

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            Debug.Assert(string.Join(" ", Solution(1)) == "1", "1");
            Debug.Assert(string.Join(" ", Solution(5)) == "1 2 4 5", "1 3 4 5");
            Debug.Assert(string.Join(" ", Solution(96234)) == "1 3 9 10 11 22 66 198 594 1782 5346 16038 16039 32078 96234", "1 3 9 10 11 22 66 198 594 1782 5346 16038 16039 32078 96234");
#else
            var amount = ParseInputs();
            var solution = Solution(amount);
            Console.WriteLine("{0}", solution.Length - 1);
            Console.WriteLine(string.Join(" ", solution));
#endif
        }

        private struct Step
        {
            public readonly int Value;
            public readonly int Count;

            public Step(int value, int count)
            {
                Value = value;
                Count = count;
            }

            public bool Valid => Count > 0;
        }

        private static int[] Solution(int goalNumber)
        {
            if (goalNumber == 0) return new int[0];

            var steps = new Step[goalNumber + 1];
            for (var currentNumber = 0; currentNumber <= goalNumber; ++currentNumber)
            {
                var currentStep = steps[currentNumber];
                for (var i = 0; i < Operations.Length; ++i)
                {
                    // Given the current operation, determine the next-biggest value.
                    var operation = Operations[i];
                    var nextNumber = operation(currentNumber);

                    // If the next number exceeds the target, we drop it.
                    if (nextNumber > goalNumber) continue;

                    // If the number obtained that way already has a previous number of steps that's smaller,
                    // we have already found a better solution before - so we discard the current one.
                    var previousStep = steps[nextNumber];
                    if (previousStep.Valid && previousStep.Count <= currentStep.Count + 1) continue;

                    steps[nextNumber] = new Step(currentNumber, currentStep.Count + 1);
                }
            }

            return Backtrack(goalNumber, steps);
        }

        private static int[] Backtrack<T>(int goalNumber, T steps)
            where T : IReadOnlyList<Step>
        {
            var stack = new Stack<int>();
            var number = goalNumber;
            do
            {
                stack.Push(number);
                number = steps[number].Value;
            } while (number > 0);

            return stack.ToArray();
        }

        private static int ParseInputs()
        {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            return int.Parse(input);
        }
    }
}