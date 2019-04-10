using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImprovingQuicksort
{
    internal static class Program
    {
        private static readonly Random Random = new Random();
        
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int[] values;
            ParseInputs(out values);
            var solution = FastSolution(values);
            Console.WriteLine(string.Join(" ", solution));
        }

        private static IEnumerable<int> FastSolution(int[] values)
        {
            RandomizedQuickSort(values, 0, values.Length - 1);
            return values;
        }

        /// <summary>
        /// Partitions the list between the <paramref name="left"/> and <paramref name="right"/> indices
        /// (with respect to the item at the <paramref name="left"/> index) and returns a set of pivot indices.
        /// </summary>
        /// <remarks>
        /// Partitioning is performed such that every item left of the first returned pivot index is strictly less than the
        /// value of the pivot item and all items right to the second returned pivot element are strictly greater.
        /// </remarks>
        /// <param name="a">The array to partition.</param>
        /// <param name="left">The index of the leftmost item to partition.</param>
        /// <param name="right">The index of the rightmost item to partition.</param>
        /// <returns>The new right and left indices.</returns>
        private static int[] Partition3(int[] a, int left, int right) {
            // TODO: write your code here
            var m1 = left;
            var m2 = right;
            return new []{m1, m2};
        }

        /// <summary>
        /// Partitions the list between the <paramref name="left"/> and <paramref name="right"/> indices
        /// (with respect to the item at the <paramref name="left"/> index) and returns the new pivot index.
        /// </summary>
        /// <remarks>
        /// Partitioning is performed such that every item left of the returned pivot index is less than or equal to the
        /// value of the pivot item and all items right to it are greater.
        /// </remarks>
        /// <param name="a">The array to partition.</param>
        /// <param name="left">The index of the leftmost item to partition.</param>
        /// <param name="right">The index of the rightmost item to partition.</param>
        /// <returns>The pivot index, i.e. the value of <paramref name="left"/> for the next iteration.</returns>
        private static int Partition2(int[] a, int left, int right)
        {
            var pivot = a[left];
            var pivotIndex = left;
            
            // Since the pivot is at 'left', we start one element after that.
            for (var i = left + 1; i <= right; ++i)
            {
                if (a[i] > pivot) continue;
                ++pivotIndex;
                Swap(a, i, pivotIndex);
            }

            // Move the pivot item from a[left] to its new location at the (new) pivot index, a[pivotIndex].
            Swap(a, left, pivotIndex);
            return pivotIndex;
        }
        
        private static void RandomizedQuickSort(int[] a, int l, int r)
        {
            if (l >= r)
            {
                return;
            }
            
            // Select a randomly chosen pivot element and swap it with the first
            // element in the sublist.
            var k = Random.Next(r - l + 1) + l;
            Swap(a, l, k);
            
            // Partition the list between the left and right indices
            // such that every element left of the partition is smaller than the pivot element.
            // TODO: use partition3
            var m = Partition2(a, l, r);
            
            // Descend into left branch.
            RandomizedQuickSort(a, l, m - 1);
            
            // Descend into right branch.
            RandomizedQuickSort(a, m + 1, r);
        }

        private static void Swap<T>(T array, int left, int right)
            where T: IList<int>
        {
            var t = array[left];
            array[left] = array[right];
            array[right] = t;
        }
        
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