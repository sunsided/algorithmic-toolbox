using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Project
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int[] values, queries;
            ParseInputs(out values, out queries);
            var solution = FastSolution(values, queries);
            Console.WriteLine(string.Join(" ", solution));
        }

        private static IEnumerable<int> FastSolution(int[] values, int[] queries)
        {
            return queries.Select(query => BinarySearch(query, values, 0, values.Length - 1));
        }

        private static int BinarySearch(int query, int[] values, int start, int end)
        {
            Debug.Assert(start < values.Length, "start < values.Length");
            Debug.Assert(end < values.Length, "end < values.Length");
            Debug.Assert(start <= end, "start <= end");

            // Termination criteria
            if (query == values[start]) return start;
            if (query == values[end]) return end;
            if (query < values[start]) return -1;
            if (query > values[end]) return -1;
            if (start == end)
            {
                if (values[start] == query) return start;
                return -1;
            }
            
            // Partition and call recursively
            var pivotIndex = (start + end) / 2;
            var pivot = values[pivotIndex];
            if (query == pivot) return pivotIndex;
            if (query < pivot)
            {
                end = pivotIndex;
            }
            else
            {
                start = pivotIndex + 1;
            }
            
             return BinarySearch(query, values, start, end);
        }

        private static void ParseInputs(out int[] values, out int[] queries)
        {
            // values to search in
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var inputs = input.Split();
            var n = int.Parse(inputs[0]);
            Debug.Assert(inputs.Length == n + 1, "values.Length == n + 1");
            values = new int[n];
            for (var i = 0; i < n; ++i)
            {
                values[i] = int.Parse(inputs[i + 1]);
            }
            
            // values to search for
            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            inputs = input.Split();
            n = int.Parse(inputs[0]);
            Debug.Assert(inputs.Length == n + 1, "values.Length == n + 1");
            queries = new int[n];
            for (var i = 0; i < n; ++i)
            {
                queries[i] = int.Parse(inputs[i + 1]);
            }
        }
    }
}