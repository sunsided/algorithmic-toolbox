using System;
using System.Diagnostics;

namespace Week3.MoneyChange
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int n;
            ParseInputs(out n);
            var solution = FastSolution(n);
            Console.WriteLine(solution);
        }

        private static int FastSolution(int n)
        {
            var coins = 0;
            while (n >= 10)
            {
                n -= 10;
                ++coins;
            }

            while (n >= 5)
            {
                n -= 5;
                ++coins;
            }

            while (n >= 1)
            {
                --n;
                ++coins;
            }

            return coins;
        }

        private static void ParseInputs(out int n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            n = int.Parse(values[0]);
        }
    }
}