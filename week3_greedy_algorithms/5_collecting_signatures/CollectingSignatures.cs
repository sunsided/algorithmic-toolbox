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
            TimeRange[] times;
            ParseInputs(out times);
            var solution = FastSolution(times);
            
            Console.WriteLine(solution.Count);
            Console.WriteLine(string.Join(" ", solution));
        }
        
        private static List<long> FastSolution(IReadOnlyList<TimeRange> inputTimes)
        {
            var times = inputTimes
                .OrderBy(x => x.From)
                .ThenBy(x => x.Length)
                .ToList();
            var timePoints = new List<long>();

            // Sanity check.
            if (times.Count == 0) return timePoints;
            
            // If the list is now empty, this means that visiting this
            // person is the last action we have to take.
            if (times.Count == 1)
            {
                // For this element, the selected time point can be any out
                // of the element's time range. We arbitrarily select the
                // earliest possible time.
                return new List<long> {times[0].From};
            }

            // We start at the beginning of the list and take the first element
            // as the constraint for which to find overlaps.
            while (times.Count > 0)
            {
                var pivot = Take(times, 0);
                var constraint = new TimeRange(pivot.From, pivot.To);

                // Scan through all (relevant) elements of the remaining set.
                // For each element, determine the overlap with the pivot constraint.
                // If it exists, use this as the new constraint and continue until no new overlap can be found.
                // After this loop, the obtained constraint is the time segment all
                // matched elements overlapped in.
                while (times.Count > 0)
                {
                    var candidate = times[0];
                    TimeRange newConstraint;
                    if (!IsOverlapping(ref constraint, ref candidate, out newConstraint)) break;

                    times.RemoveAt(0);
                    constraint = newConstraint;
                }
                
                // We now arbitrarily select the start time.
                timePoints.Add(constraint.From);
            }

            return timePoints;
        }

        private static bool IsOverlapping(ref TimeRange constraint, ref TimeRange element, out TimeRange overlap)
        {
            overlap = constraint;
            
            // If element is entirely on the left, mismatch.
            if (element.To < constraint.From) return false;
            
            // If element is entirely on the right, mismatch.
            if (element.From > constraint.To) return false;

            overlap = new TimeRange(
                Math.Max(constraint.From, element.From),
                Math.Min(constraint.To, element.To));
            return true;
        }

        /// <summary>
        /// Takes the element at the specified <paramref name="index"/> from the list of <paramref name="times"/>,
        /// removes it from the list and returns it.
        /// </summary>
        /// <param name="times">The list to take and remove the element from.</param>
        /// <param name="index">The index of the element.</param>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <returns>The element</returns>
        private static TimeRange Take<T>(T times, int index)
            where T: IList<TimeRange>
        {
            Debug.Assert(times.Count > index, "times.Count > index");
            var element = times[index];
            times.RemoveAt(index);
            return element;
        }

        private static void ParseInputs(out TimeRange[] times) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input.Trim());
            
            times = new TimeRange[n];
            for (var i = 0; i < n; ++i)
            {
                input = Console.ReadLine();
                Debug.Assert(input != null, "input != null");
                var values = input.Split();
                
                var @from =  int.Parse(values[0]);
                var to =  int.Parse(values[1]);
                times[i] = new TimeRange(@from, to); 
            }
        }

        [DebuggerDisplay("{From} .. {To} (length {Length})")]
        private struct TimeRange
        {
            public readonly long From;
            public readonly long To;
            public readonly long Length;

            public TimeRange(long @from, long to)
            {
                From = @from;
                To = to;
                Length = to - @from + 1;
            }
        }
    }
}