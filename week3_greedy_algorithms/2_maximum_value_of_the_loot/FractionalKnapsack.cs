using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week3.MaximumValueOfTheLoot
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int n, w;
            int[] values, weights;
            ParseInputs(out n, out w, out values, out weights);
            var solution = FastSolution(n, w, values, weights);
            Console.WriteLine(solution);
        }

        private static double FastSolution(int n, int w, IReadOnlyList<int> values, IReadOnlyList<int> weights)
        {
            var items = new Item[n];
            for (var i = 0; i < n; ++i)
            {
                items[i] = new Item(values[i], weights[i]);
            }

            items = items
                .OrderByDescending(v => v.ValuePerPart)
                .ToArray();

            var token = 0;
            var totalValue = 0D;
            while (w > 0 && token < items.Length)
            {
                var item = items[token];
                if (item.Amount <= w)
                {
                    totalValue += item.Value;
                    w -= item.Amount;
                    ++token;
                    continue;
                }

                totalValue += item.Value * ((double)w / item.Amount);
                break;
            }

            return totalValue;
        }

        private static void ParseInputs(out int n, out int w, out int[] itemValues, out int[] itemWeights) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            n = int.Parse(values[0]);
            w = int.Parse(values[1]);

            itemValues = new int[n];
            itemWeights = new int[n];
            for (var i = 0; i < n; ++i)
            {
                input = Console.ReadLine();
                Debug.Assert(input != null, "input != null");
                values = input.Split();
                itemValues[i] = int.Parse(values[0]);
                itemWeights[i] = int.Parse(values[1]);
            }
        }

        [DebuggerDisplay("{Value} x{Amount} ({ValuePerPart} per part)")]
        private struct Item
        {
            public readonly double ValuePerPart;
            public readonly int Value;
            public readonly int Amount;

            public Item(int value, int amount)
            {
                Value = value;
                Amount = amount;
                ValuePerPart = (double)value / amount;
            }
        }
    }
}