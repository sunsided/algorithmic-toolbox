#define PARTITION3

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ImprovingQuicksort
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int[] values;
            ParseInputs(out values);
            //values = new[] {2, 3, 9, 2, 9};
            //values = new[] {2, 1, 3, 1, 2};
            //values = new[] {2, 4, 1, 3, 5};
            //values = new[] {2, 2, 1};
            var solution = FastSolution(values);
            Console.WriteLine(string.Join(" ", solution));
        }

        private static int FastSolution(int[] values)
        {
            var result = MergeSort(values, 0, values.Length - 1);
            Debug.Assert(values.Zip(values.Skip(1), (a, b) => a <= b).All(x => x));
            return result;
        }
        
        private static int MergeSort(int[] values, int left, int right)
        {
            if (left >= right)
            {
                return 0;
            }

            var pivotIndex = (left + right) / 2;
            var numInvLeft = MergeSort(values, left, pivotIndex);
            var numInvRight = MergeSort(values, pivotIndex + 1, right);
            var numInvMerge = 0;

            var l = left;
            var r = pivotIndex + 1;
            
            var i = 0;
            var newList = new int[right - left + 1];
            
            while (l <= pivotIndex && r <= right)
            {
                if (values[l] <= values[r])
                {
                    newList[i++] = values[l++];
                }
                else
                {
                    newList[i++] = values[r++];
                    numInvMerge += pivotIndex - l + 1;
                }
            }

            while (l <= pivotIndex)
            {
                newList[i++] = values[l++];
            }
            
            while (r <= right)
            {
                newList[i++] = values[r++];
            }
            
            Array.Copy(newList, 0, values, left, newList.Length);

            return numInvMerge + numInvLeft + numInvRight;
        }

#if RELEASE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void Swap<T>(T values, int left, int right)
            where T: IList<int>
        {
            Debug.Assert(left >= 0, "left >= 0");
            Debug.Assert(right >= 0, "right >= 0");
            Debug.Assert(left < values.Count, "left < values.Count");
            Debug.Assert(right < values.Count, "right < values.Count");
            
            if (left == right) return;
            var t = values[left];
            values[left] = values[right];
            values[right] = t;
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