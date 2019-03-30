using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace project
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            long n;
            int m;
            ParseInputs(out n, out m);
            var solution = FastSolution(n, m);
            Console.WriteLine(solution);
        }

        private static long FastSolution(long n, int m)
        {
            var pisanoPeriod = GetPisanoPeriod(n, m);

            var remainder = n % pisanoPeriod.Count;
            var values = FibonacciSequence(remainder, m).ToList();
            return values.Last();
        }

        private static List<long> GetPisanoPeriod(long n, int m)
        {
            Debug.Assert(n >= 1, "n >= 1");
            Debug.Assert(m >= 2, "m >= 2");

            var pisanoPeriod = new List<long>();
            foreach (var value in FibonacciSequence(n, m))
            {
                pisanoPeriod.Add(value);

                var last = pisanoPeriod.Count - 1;
                if (last <= 4) continue;
                
                var repeats = pisanoPeriod[last - 2] == 0 &&
                              pisanoPeriod[last - 1] == 1 &&
                              pisanoPeriod[last] == 1;
                if (!repeats) continue;
                
                pisanoPeriod.RemoveAt(last);
                pisanoPeriod.RemoveAt(last - 1);
                pisanoPeriod.RemoveAt(last - 2);
                break;
            }

            return pisanoPeriod;
        }

        private static IEnumerable<long> FibonacciSequence(long n, long modulo)
        {
            Debug.Assert(modulo > 2, "modulo > 2");

            if (n >= 0) yield return 0;
            if (n >= 1) yield return 1;
            if (n <= 1) yield break;

            var close = 1L;
            var far = 0L;
            for (var i = 0; i < n-1; ++i)
            {
                var @new = close + far;
                far = close;
                close = @new % modulo;

                // Taking the modulo directly as part of the calculation is crucial
                // to prevent overflows e.g. for inputs such as n=2816213588, m=239
                // (which should result in 151).
                yield return close;
            }
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static void ParseInputs(out long n, out int m)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split(); 
            n = long.Parse(values[0]);
            m = int.Parse(values[1]);
        }
    }
}