using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week5.LongestCommonSubsequence
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
#if TESTING
            Testing.Run();
#else
            RunMain();
#endif
        }

        public static long Solution(List<long> source, List<long> target)
        {
            List<long> ignored = null;
            return Solution(source, target, ref ignored);
        }

        public static long Solution(List<long> source, List<long> target, ref List<long> lcs)
        {
            // See https://www.youtube.com/watch?v=HgUOWB0StNE for a nice video explanation on the LCS problem.

            var distances = InitializeLCSMatrix(source, target);
            for (var j = 1; j <= target.Count; ++j)
            {
                for (var i = 1; i <= source.Count; ++i)
                {
                    var lcsWhenMatched = distances[i - 1, j - 1] + 1;
                    var lcsWhenMismatched = Math.Max(distances[i, j - 1], distances[i - 1, j]);

                    var itemsMatch = source[i - 1] == target[j - 1];
                    distances[i, j] = itemsMatch
                        ? lcsWhenMatched
                        : lcsWhenMismatched;
                }
            }

            // For debugging purposes, we're allowing to extract one of the detected LCS's.
            if (lcs != null)
            {
                lcs.Clear();
                lcs.AddRange(Backtrack(distances, source, target));
            }

            // We can derive the length of the LCS from looking up the bottom-right value in the LCS matrix:
            return distances[source.Count, target.Count];
        }

        private static IEnumerable<long> Backtrack(int[,] distances, List<long> source, List<long> target)
        {
            var i = source.Count;
            var j = target.Count;

            // We initialize a stack for reversing the list later on;
            var fullLcs = distances[source.Count, target.Count];
            var stack = new Stack<long>(fullLcs);

            // Once we hit a zero-length LCS, there's no point in searching further.
            while (distances[i, j] > 0)
            {
                // Determine the LCS for the current sub-problem.
                var lcs = distances[i, j];
                var topLcs = distances[i, j - 1];
                var leftLcs = distances[i - 1, j];

                // Determine whether the token at (i, j) is a part of the LCS.
                var maxOfTopAndLeft = Math.Max(leftLcs, topLcs);
                var isPartOfLCS = lcs != maxOfTopAndLeft;
                if (isPartOfLCS)
                {
                    // Pick the matching token.
                    Debug.Assert(source[i - 1] == target[j - 1], "source[i - 1] == target[j - 1]");
                    stack.Push(source[i - 1]);

                    // Move up diagonally.
                    --i;
                    --j;
                    continue;
                }

                // If the current LCS length came from "up", move up;
                // if it came from "left", move left. If the current LCS length is
                // equal to left or up, pick any of them.
                if (lcs == topLcs)
                {
                    --j;
                }
                else if (lcs == leftLcs)
                {
                    --i;
                }
            }

            return stack;
        }

        private static int[,] InitializeLCSMatrix(ICollection source, ICollection target)
        {
            // Note that contrary to the Edit Distance problem, we
            // need the first row and column to be initialized with zero.
            return new int[source.Count + 1, target.Count + 1];
        }

        private static void RunMain()
        {
            List<long> source, target;
            ParseInputs(out source, out target);

            var solution = Solution(source, target);
            Console.WriteLine(solution);
        }

        private static void ParseInputs(out List<long> first, out List<long> second)
        {
            {
                var count = int.Parse(Console.ReadLine());
                var line = Console.ReadLine();
                first = line.Split().Select(long.Parse).ToList();
                Debug.Assert(first.Count == count, "first.Count == count");
            }

            {
                var count = int.Parse(Console.ReadLine());
                var line = Console.ReadLine();
                second = line.Split().Select(long.Parse).ToList();
                Debug.Assert(second.Count == count, "second.Count == count");
            }
        }
    }
}