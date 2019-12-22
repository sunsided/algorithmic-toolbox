using System;
using System.Diagnostics;

namespace Week2.LeastCommonMultiple
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int a, b;
            ParseInputs(out a, out b);
            var solution = FastSolution(a, b);
            Console.WriteLine(solution);
        }

        private static long FastSolution(long a, long b)
        {
            OrderBigSmall(ref a, ref b);

            var gcd = GreatestCommonDivisor(a, b);
            return (a / gcd) * b;
        }

        private static long GreatestCommonDivisor(long a, long b)
        {
            Debug.Assert(a >= b, "a >= b");
            do
            {
                var temp = a / b;
                var a_ = a;
                var b_ = b;
                a = b_;
                b = a_ - temp * b_;

                if (a == b || b == 1)
                {
                    return b;
                }

                if (b == 0)
                {
                    return a;
                }
            } while (true);
        }

        private static void OrderBigSmall(ref long a, ref long b)
        {
            if (a >= b) return;
            var t = a;
            a = b;
            b = t;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static void ParseInputs(out int a, out int b)
        {
            // Read number of inputs
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            a = int.Parse(values[0]);
            b = int.Parse(values[1]);
        }
    }
}