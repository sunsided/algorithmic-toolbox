using System;
using System.Diagnostics;

namespace Week2.LastDigitOfSumOfSquaresOfFibonacciNumbers
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            long n;
            ParseInputs(out n);
            var solution = FastSolution(n);
            Console.WriteLine(solution);
        }

        private static int FastSolution(long n)
        {
            // We make use of Sum F(1..n)^2 = F(n+1) * F(n)
            // For modulo 100, the Pisano period length is 300.
            const int pisanoPeriod = 300;
            var remainderN = n % pisanoPeriod;

            // F(n) = F(n+2) - F(1)
            var a = FastFibonacciRecursiveModulo(remainderN, 10);
            var b = FastFibonacciRecursiveModulo(remainderN + 1, 10);

            // Add a small amount of hacking ... to counter negative values.
            var result = (b * a) % 10;
            if (result < 0) result = 10 + result;
            return result;
        }

        private static int FastFibonacciRecursiveModulo(long n, int modulo)
        {
            // Follows method from here: https://www.youtube.com/watch?v=ruIwND9ytpE
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

        private static void ParseInputs(out long n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            n = long.Parse(values[0]);
        }
    }
}