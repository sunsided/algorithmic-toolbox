using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Week5.EditDistance
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
#else
            string first, second;
            ParseInputs(out first, out second);
            var solution = Solution(first, second);
            Console.WriteLine(solution);
#endif
        }

        private static int Solution(string first, string second)
        {
            throw new NotImplementedException();
        }

        private static void ParseInputs(out string first, out string second)
        {
            first = Console.ReadLine();
            second = Console.ReadLine();
        }
    }
}