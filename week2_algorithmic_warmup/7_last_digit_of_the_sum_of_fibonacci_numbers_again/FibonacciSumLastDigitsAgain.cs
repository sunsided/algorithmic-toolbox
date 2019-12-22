using System;
using System.Diagnostics;

namespace Week2.LastDigitOfSumOfFibonacciNumbersAgain
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            long m, n;
            ParseInputs(out m, out n);
            var solution = FastSolution(m, n);
            Console.WriteLine(solution);
        }

        private static int FastSolution(long m, long n)
        {
            Debug.Assert(m <= n, "m <= n");
            // We make use of F(n) = F(n+2) - F(1). However, this is infeasible for large inputs.
            // For this, we apply the Pisano hack. In theory, mod 10 would suffice since we need the last digit
            // only, however, with mod 10, the above assumption F(n) mod 10 = F(n+2) mod 10 - F(1) mod 10 breaks
            // for e.g. n=13, since F(15) mod 10 = 0. Because of this, we use mod 100.

            // For modulo 100, the Pisano period length is 300.
            const int pisanoPeriod = 300;
            var remainderM = m % pisanoPeriod;
            var remainderN = n % pisanoPeriod;

            // F(n) = F(n+2) - F(1)
            var firstSum = FastFibonacciRecursiveModulo(remainderM + 1, 10);
            var secondSum = FastFibonacciRecursiveModulo(remainderN + 2, 10);

            // Add a small amount of hacking ... to counter negative values.
            var result = (secondSum - firstSum) % 10;
            if (result < 0) result = 10 + result;
            return result;
        }

        private static int FastFibonacciRecursiveModulo(long n, int modulo)
        {
            // Follows method 6 from here: https://www.geeksforgeeks.org/program-for-nth-fibonacci-number/
            // We additionally take mod 10 since we only need the last digit anyway.
            Debug.Assert(modulo % 10 == 0, "modulo % 10 == 0");
            if (n <= 1) return (int)n;

            var even = (n & 1) == 0;
            if (even)
            {
                var k = n / 2;

                var fibKm1 = FastFibonacciRecursiveModulo(k - 1, modulo);
                var fibK = FastFibonacciRecursiveModulo(k, modulo);
                return (2 * fibKm1 + fibK) * fibK % modulo;
            }
            else
            {
                var k = (n + 1) / 2;
                var fibKm1 = FastFibonacciRecursiveModulo(k - 1, modulo);
                var fibK = FastFibonacciRecursiveModulo(k, modulo);
                return (fibK * fibK + fibKm1 * fibKm1) % modulo;
            }
        }

        private static void ParseInputs(out long m, out long n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            m = long.Parse(values[0]);
            n = long.Parse(values[1]);
        }
    }
}