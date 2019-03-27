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
            const int minInputs = 1;
            const int maxInputs = 2000000000;

            var random = new Random(0);
            while (true)
            {
                var a = random.Next() % (maxInputs - minInputs + 1) + minInputs;
                var b = random.Next() % (maxInputs - minInputs + 1) + minInputs;
                
                // Compare solutions
                try
                {
                    var naive = NaiveSolution(a, b);
                    var faster = FastSolution(a, b);
                    if (naive == faster)
                    {
                        Console.Write(".");
                        continue;
                    }

                    Console.WriteLine();
                    Console.Error.WriteLine("FAIL at a={0}, b={1}", a, b);
                    Console.Error.WriteLine("Naive: {0}, Faster: {1}", naive, faster);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.Error.WriteLine("EXCEPTION at a={0}, b={1}", a, b);
                    Console.Error.WriteLine(e);
                    return;
                }
            }
#else
            int a, b;
            ParseInputs(out a, out b);
            var solution = FastSolution(a, b);
            Console.WriteLine(solution);
#endif
        }

#if TESTING
        
        private static long NaiveSolution(long a, long b)
        {
            if (a == b) return a;
            OrderBigSmall(ref a, ref b);

            do
            {
                var temp = a - b;
                if (temp <= b)
                {
                    return NaiveSolution(b, temp);
                }
                a = temp;
            } while (true);
        }

#endif

        private static long FastSolution(long a, long b)
        {
            OrderBigSmall(ref a, ref b);
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