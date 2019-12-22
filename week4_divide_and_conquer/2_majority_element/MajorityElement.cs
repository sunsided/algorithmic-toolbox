using System;
using System.Diagnostics;
using System.Linq;

namespace Week4.MajorityElement
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int[] values;
            ParseInputs(out values);
            var solution = FastSolution(values);
            Console.WriteLine(solution ? "1" : "0");
        }

        private static bool FastSolution(int[] values)
        {
            // First, sort the values by using any divide-and-conquer O(n log n) algorithm.
            var valueList = values.ToList();
            valueList.Sort();

            // Next, select the element with a majority count in O(n).
            // Since O(n log n) > O(n), the sorting dominates the complexity.
            var midpoint = values.Length / 2;
            var pivot = valueList[0];
            var count = 1;
            for (var i = 1; i < valueList.Count; ++i)
            {
                var element = valueList[i];
                if (pivot == element)
                {
                    ++count;
                    if (count > midpoint) return true;
                    continue;
                }

                pivot = element;
                count = 1;
            }

            return false;
        }

#if WTF

        private static readonly int NoElement = -1;

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            // int[] values = {0, 0, 0, 6, 6, 6};
            // int[] values = {2, 3, 9, 2, 2};
            // int[] values = {3, 7, 7, 2, 7, 7};
            // int[] values = {512766168, 717383758, 5, 126144732, 5, 573799007, 5, 5, 5, 405079772};
            int[] values;
            ParseInputs(out values);
            var solution = FastSolution(values);
            Console.WriteLine(solution ? "1" : "0");
        }

        private static bool FastSolution(int[] values)
        {
            var element = FindMajorityElement(values, 0, values.Length - 1);

            // According to the problem definition, all elements are nonnegative.
            return element >= 0;
        }

        private static int FindMajorityElement(int[] values, int start, int end)
        {
            var length = end - start + 1;

            if (length <= 1)
            {
                return NoElement;
            }

            if (length == 2)
            {
                if (values[start] == values[end])
                {
                    return values[start];
                }

                if (values[start] > values[end])
                {
                    Swap(values, start, end);
                }

                return NoElement;
            }

            var pivotIndex = (start + end) / 2;
            FindMajorityElement(values, start, pivotIndex);
            FindMajorityElement(values, pivotIndex + 1, end);

            // Merge the results.
            // TODO: This part may be broken.
            var l = start;
            var r = pivotIndex + 1;
            for (var i = start; i <= end && r <= end; ++i)
            {
                var value = values[i];
                if (value > values[r])
                {
                    Swap(values, i, r);
                    ++r;
                }
            }

            // Find the majority candidate.
            // TODO: Ideally, we can do this as part of the merge sort.
            var majorityCandidate = values[start];
            var majorityCount = 1;
            var midpoint = length / 2;
            for (var i = start + 1; i <= end; ++i)
            {
                if (values[i] == majorityCandidate)
                {
                    ++majorityCount;
                    if (majorityCount > midpoint) break;
                    continue;
                }

                majorityCount = 1;
                majorityCandidate = values[i];
            }

            if (majorityCount > midpoint)
            {
                return majorityCandidate;
            }

            return NoElement;
        }

        private static void Swap(int[] values, int lhs, int rhs)
        {
            var tmp = values[lhs];
            values[lhs] = values[rhs];
            values[rhs] = tmp;
        }

#endif

        private static void ParseInputs(out int[] values)
        {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input);

            // values to search in
            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var inputs = input.Split();

            values = new int[n];
            for (var i = 0; i < n; ++i)
            {
                values[i] = int.Parse(inputs[i]);
            }
        }
    }
}