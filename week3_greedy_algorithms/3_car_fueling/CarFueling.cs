using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Project
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            int cityDistance, fuelCapacity;
            int[] stopDistances;
            ParseInputs(out cityDistance, out fuelCapacity, out stopDistances);
            var solution = FastSolution(cityDistance, fuelCapacity, stopDistances);
            Console.WriteLine(solution);
        }
        
        private static double FastSolution(int cityDistance, int fuelCapacity, IReadOnlyList<int> stopDistances)
        {
            // If there is no stop in between the cities we can either directly reach the goal
            // or are unable to reach it at all.
            if (stopDistances.Count == 0)
            {
                return cityDistance < fuelCapacity ? 0 : -1;
            }

            var distances = new List<int>(stopDistances.Count + 2) {0};
            distances.AddRange(stopDistances);
            distances.Add(cityDistance);
           
            var finalPosition = stopDistances.Count + 1;
           
            var currentPosition = 0;
            var numStops = 0;
            while (currentPosition < distances.Count)
            {
                var traveledDistance = distances[currentPosition];
                var reachablePosition = currentPosition;
                for (var i = currentPosition + 1; i < distances.Count; ++i)
                {
                    var distance = distances[i] - traveledDistance;
                    if (distance > fuelCapacity) break;
                    reachablePosition = i;
                }

                // If the position didn't change, the goal is unreachable.
                if (currentPosition == reachablePosition)
                {
                    return -1;
                }
                
                // If we reached the final position, we have refilled often enough.
                if (reachablePosition == finalPosition)
                {
                    return numStops;
                }

                // If we are able to reach a new position that is not the goal,
                // it means we can refill and re-start our search from there.
                if (currentPosition != reachablePosition)
                {
                    ++numStops;
                    currentPosition = reachablePosition;
                }
            }
            
            return numStops;
        }

        private static void ParseInputs(out int distance, out int fuelCapacity, out int[] stopDistances) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            distance = int.Parse(input.Trim());
            
            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            fuelCapacity = int.Parse(input.Trim());
            
            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var n = int.Parse(input.Trim());

            input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            var values = input.Split();
            
            stopDistances = new int[n];
            for (var i = 0; i < n; ++i)
            {
                stopDistances[i] = int.Parse(values[i]);
            }
        }
    }
}