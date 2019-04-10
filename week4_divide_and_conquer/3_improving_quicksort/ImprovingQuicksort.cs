#define PARTITION3

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImprovingQuicksort
{
    internal static class Program
    {
        private static readonly Random Random = new Random(1);
        
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

#if PARTITION3

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
        /// <param name="pivotIndex">The pivot index.</param>
        /// <returns>The new right and left indices.</returns>
        private static int[] Partition3(int[] a, int left, int right, int pivotIndex) {
            Swap(a, left, pivotIndex);
            
            var pivot = a[left];
            var pivotLeftIndex = left;
            var pivotRightIndex = left;
            
            // First we do a 2-way partitioning step where we move all items greater than or equal
            // to the element of interest to the right of the pivot.
            // Since the pivot is at 'left' initially, we start one element after that.
            for (var i = left + 1; i <= right; ++i)
            {
                if (a[i] > pivot) continue;
                
                ++pivotRightIndex;
                Swap(a, i, pivotRightIndex);
            }
            
            // Next, we subdivide the partitioned space left of the pivot and
            // re-partition it such that all elements strictly less than the pivot
            // are ordered to the left of the pivot.
            for (var i = left + 1; i <= pivotRightIndex; ++i)
            {
                if (a[i] >= pivot) continue;
                
                ++pivotLeftIndex;
                Swap(a, i, pivotLeftIndex);
            }
            
            // Move the pivot item from a[left] to its new location at the (new) pivot index, a[pivotIndex].
            Swap(a, left, pivotLeftIndex);
            
            // return new []{m1, m2};
            return new[] {pivotLeftIndex, pivotRightIndex};
        }

#else

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
        /// <param name="pivotIndex">The pivot index.</param>
        /// <returns>The pivot index, i.e. the value of <paramref name="left"/> for the next iteration.</returns>
        private static int Partition2(int[] a, int left, int right, int pivotIndex)
        {
            Swap(a, left, pivotIndex);
            
            var pivot = a[left];
            pivotIndex = left;
            
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

#endif

        private static void RandomizedQuickSort(int[] a, int l, int r)
        {
            if (l >= r)
            {
                return;
            }
            
            // Select a randomly chosen pivot element and swap it with the first
            // element in the sublist.
            
            //var k = Random.Next(r - l + 1) + l;
            var k = IndexOfMedianOfThree(a, l, (l + r) / 2, r);
            
            // Partition the list between the left and right indices
            // such that every element left of the partition is smaller than the pivot element.
#if PARTITION3
            var m = Partition3(a, l, r, k);

            // Descend into left branch.
            RandomizedQuickSort(a, l, m[0] - 1);
            
            // Descend into right branch.
            RandomizedQuickSort(a, m[1] + 1, r);
#else
            var m = Partition2(a, l, r, k);

            // Descend into left branch.
            RandomizedQuickSort(a, l, m - 1);
            
            // Descend into right branch.
            RandomizedQuickSort(a, m + 1, r);
#endif
        }

        private static void Swap<T>(T array, int left, int right)
            where T: IList<int>
        {
            var t = array[left];
            array[left] = array[right];
            array[right] = t;
        }
        
        private static int IndexOfMedianOfThree<T>(T array, int a, int b, int c)
            where T: IReadOnlyList<int>
        {
            Debug.Assert(a <= b, "a < b");
            Debug.Assert(b <= c, "a < b");
            
            var va = array[a];
            var vb = array[b];
            var vc = array[c];

            if (va == vb && vb == vc)
            {
                return Random.Next(a, c + 1);
            }
            
            if ((va > vb) != (va > vc))
            {
                return a;
            }

            if ((vb > va) != (vb > vc))
            {
                return b;
            }

            return c;
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