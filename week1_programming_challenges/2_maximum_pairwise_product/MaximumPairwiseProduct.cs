using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week1.MaximumPairwiseProduct
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            const int minInputs = 2;
            const int maxInputs = 100;
            const int maxValue = 2*100000;

            var random = new Random(0);
            while (true)
            {
                // Generate at least two numbers.
                var n = random.Next() % (maxInputs - minInputs + 1) + minInputs;

                // Generate random digits
                var numbers = new int[n];
                for (var i = 0; i < n; ++i)
                {
                    numbers[i] = random.Next() % maxValue;
                }

                // Compare solutions
                var naive = NaiveSolution(numbers);
                var faster = FastSolution(numbers);
                if (naive == faster)
                {
                    Console.Write(".");
                    continue;
                };

                Console.WriteLine();
                Console.Error.WriteLine("FAIL at n={0}", n);
                Console.Error.WriteLine("Naive: {0}, Faster: {1}", naive, faster);
                Console.Error.WriteLine("Inputs: {0}", string.Join(' ', numbers));
                return;
            }
#else
            int n;
            var numbers = ParseInputs(out n);
            var solution = FastSolution(numbers);
            Console.WriteLine(solution);
#endif
        }

#if TESTING

        private static long NaiveSolution(int[] numbers)
        {
            var bigger = -1;
            var biggerIdx = -1;
            for (var i = 0; i < numbers.Length; ++i)
            {
                if (bigger >= numbers[i]) continue;
                bigger = numbers[i];
                biggerIdx = i;
            }

            var smaller = -1;
            for (var i = 0; i < numbers.Length; ++i)
            {
                if (i == biggerIdx) continue;
                smaller = Math.Max(numbers[i], smaller);
            }

            return (long) smaller * bigger;
        }

#endif

        private static long FastSolution(IEnumerable<int> numbers)
        {
            var small = -1;
            var big = -1;
            foreach (var t in numbers)
            {
                small = Math.Max(small, t);
                if (small > big)
                {
                    Swap(ref small, ref big);
                }
            }

            return (long)small * big;
        }

        private static void Swap(ref int a, ref int b)
        {
            var t = a;
            a = b;
            b = t;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static int[] ParseInputs(out int n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            n = int.Parse(input);

            // Read inputs
            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var tokens = input.Split(' ');
            Debug.Assert(tokens.Length == n, "tokens.Length == n");

            // Convert inputs for algorithm
            var numbers = tokens.Select(int.Parse).ToArray();
            return numbers;
        }
    }
}