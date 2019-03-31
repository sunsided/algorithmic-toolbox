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
            long n;
            ParseInputs(out n);
            var solution = FastSolution(n);
            Console.WriteLine(solution.Count);
            Console.WriteLine(string.Join(" ", solution));
        }
        
        private static List<long> FastSolution(long candies)
        {
            var prices = new List<long>();
            if (candies == 0) return prices;
            
            var minAmount = 0L;
            var remaining = candies;

            while (true)
            {
                // We increase the minimum amount as slowly as possible.
                ++minAmount;
                
                // If this move results in a remaining set being exactly the same size (or, technically
                // smaller as well), the next price would be the same as (or less than) the current spot.
                // If this happens, we terminate with the remaining sum (which is larger than the previous price).
                if ((remaining - minAmount) <= minAmount)
                {
                    prices.Add(remaining);
                    break;
                } 
                
                prices.Add(minAmount);
                remaining -= minAmount;
            }
            
            return prices;
        }


        private static void ParseInputs(out long n) {
            var input = Console.ReadLine();
            Debug.Assert(input != null, "input != null");
            n = long.Parse(input.Trim());
        }
    }
}