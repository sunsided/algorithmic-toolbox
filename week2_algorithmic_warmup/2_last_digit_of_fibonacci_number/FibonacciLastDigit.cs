using System;

namespace Week2.LastDigitOfFibonacciNumber
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            const int minInputs = 0;
            const int maxInputs = 45;

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
            int n;
            ParseInputs(out n);
            var solution = FastSolution(n);
            Console.WriteLine(solution);
#endif
        }

#if TESTING

        private static long NaiveSolution(int n)
        {
            if (n <= 1) return n;
            var a = NaiveSolution(n - 1);
            var b = NaiveSolution(n - 2);
            return (a + b) % 10;
        }

#endif

        private static long FastSolution(int n)
        {
            if (n <= 1) return n;

            var close = 1L;
            var far = 0L;
            for (var i = 0; i < (n-1); ++i)
            {
                var @new = (close + far) % 10;
                far = close;
                close = @new;
            }

            return close;
        }

#if !TESTING

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static void ParseInputs(out int n)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            n = int.Parse(input);
        }

#endif
    }
}