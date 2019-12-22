using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week3.MaximumAdvertisementRevenue
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int[] profitPerClick, clicksPerDay;
            ParseInputs(out profitPerClick, out clicksPerDay);
            var solution = FastSolution(profitPerClick, clicksPerDay);
            Console.WriteLine(solution);
        }

        private static long FastSolution(IReadOnlyList<int> profitPerClick, IReadOnlyList<int> clicksPerDay)
        {
            Debug.Assert(profitPerClick.Count == clicksPerDay.Count, "profitPerClick.Count == clicksPerDay.Count");

            return profitPerClick
                .OrderByDescending(x => x)
                .Zip(clicksPerDay.OrderByDescending(x => x),
                    (a, b) => (long)a * b)
                .Sum();
        }

        private static void ParseInputs(out int[] profitPerClick, out int[] clicksPerDay) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input.Trim());

            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            profitPerClick = new int[n];
            for (var i = 0; i < n; ++i)
            {
                profitPerClick[i] = int.Parse(values[i]);
            }

            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            values = input.Split();
            clicksPerDay = new int[n];
            for (var i = 0; i < n; ++i)
            {
                clicksPerDay[i] = int.Parse(values[i]);
            }
        }
    }
}