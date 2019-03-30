using System;
using System.Diagnostics;

namespace project
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            const int minInputs = 0;
            const int maxInputs = 20;

            var random = new Random(0);
            while (true)
            {
                var n = random.Next() % (maxInputs - minInputs + 1) + minInputs;
                
                // Compare solutions
                var naive = NaiveSolution(n);
                var faster = FastSolution(n);
                if (naive == faster)
                {
                    Console.Write(".");
                    continue;
                };
                
                Console.WriteLine();
                Console.Error.WriteLine("FAIL at n={0}", n);
                Console.Error.WriteLine("Naive: {0}, Faster: {1}", naive, faster);
                return;
            }

#else
            long n;
            ParseInputs(out n);
            var solution = FastSolution(n);
            Console.WriteLine(solution);
#endif
        }

#if TESTING
        
        private static int NaiveSolution(long n)
        {
            if (n <= 1) return (int)n;

            var close = 1;
            var far = 0;
            var sumLastDigit = close + far;
            for (var i = 0; i < n-1; ++i)
            {
                var @new = close + far;
                far = close;
                close = @new % 10;
                
                // The key to this solution is again that the last digit is always
                // changing fastest, so using a modulo-10 on the values to keep them small
                // will always result in the correct last digit.
                sumLastDigit = (sumLastDigit + close) % 10;
            }

            return sumLastDigit;
        }

#endif
        
        private static int FastSolution(long n)
        {
            // We make use of F(n) = F(n+2) - F(1). However, this is infeasible for large inputs.
            // For this, we apply the Pisano hack. In theory, mod 10 would suffice since we need the last digit
            // only, however, with mod 10, the above assumption F(n) mod 10 = F(n+2) mod 10 - F(1) mod 10 breaks
            // for e.g. n=13, since F(15) mod 10 = 0. Because of this, we use mod 100.

            // For modulo 100, the Pisano period length is 300.
            const int pisanoPeriod = 300;
            var remainder = n % pisanoPeriod;

            // F(n) = F(n+2) - F(1)
            var a = FastFibonacciRecursiveModulo(remainder + 2, 100);
            const int b = 1; // FastFibonacciRecursiveMod10(1);
            return (a - b) % 10;
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

#if !TESTING
        
        private static void ParseInputs(out long n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split(); 
            n = long.Parse(values[0]);
        }

#endif
    }
}