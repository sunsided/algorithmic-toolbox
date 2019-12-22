using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Week3.MaximumSalary
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            List<string> values;
            ParseInputs(out values);
            var solution = FastSolution(values);
            Console.WriteLine(string.Join("", solution).TrimStart('0'));
        }

        private static IEnumerable<string> FastSolution(List<string> values)
        {
            values.Sort(new CompareLiteral());
            return values;
        }

        private class CompareLiteral : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                var a = x + y;
                var b = y + x;
                return string.Compare(b, a, StringComparison.Ordinal);
            }
        }

        private static void ParseInputs(out List<string> values) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input.Trim());

            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var split = input.Split();
            values = new List<string>(n);
            for (var i = 0; i < n; ++i)
            {
                values.Add(split[i]);
            }
        }
    }
}