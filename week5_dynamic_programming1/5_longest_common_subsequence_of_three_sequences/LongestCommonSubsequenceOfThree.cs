using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Week5.LongestCommonSubsequenceOfThree
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

        public static long Solution(List<long> first, List<long> second, List<long> third)
        {
            List<long> ignored = null;
            return Solution(first, second, third, ref ignored);
        }

        public static long Solution(List<long> first, List<long> second, List<long> third, ref List<long> lcs)
        {
            // See https://www.youtube.com/watch?v=HgUOWB0StNE for a nice video explanation on the LCS problem.

            var distances = InitializeLCSMatrix(first, second, third);
            for (var k = 1; k <= third.Count; ++k)
            {
                for (var j = 1; j <= second.Count; ++j)
                {
                    for (var i = 1; i <= first.Count; ++i)
                    {
                        var lcsWhenMatched = distances[i - 1, j - 1, k - 1] + 1;
                        var lcsWhenMismatched = Max(
                            distances[i - 1, j, k],
                            distances[i, j - 1, k],
                            distances[i, j, k - 1]);

                        var itemsMatch = first[i - 1] == second[j - 1] && first[i - 1] == third[k - 1];
                        distances[i, j, k] = itemsMatch
                            ? lcsWhenMatched
                            : lcsWhenMismatched;
                    }
                }
            }

            // For debugging purposes, we're allowing to extract one of the detected LCS's.
            if (lcs != null)
            {
                lcs.Clear();
                lcs.AddRange(Backtrack(distances, first, second, third));
            }

            // We can derive the length of the LCS from looking up the bottom-right value in the LCS matrix:
            return distances[first.Count, second.Count, third.Count];
        }

        private static int Max(int a, int b, int c)
        {
            return Math.Max(Math.Max(a, b), c);
        }

        private static IEnumerable<long> Backtrack(int[,,] distances, List<long> first, List<long> second, List<long> third)
        {
            var i = first.Count;
            var j = second.Count;
            var k = third.Count;

            // We initialize a stack for reversing the list later on;
            var fullLcs = distances[first.Count, second.Count, third.Count];
            var stack = new Stack<long>(fullLcs);

            // Once we hit a zero-length LCS, there's no point in searching further.
            while (distances[i, j, k] > 0)
            {
                // Determine the LCS for the current sub-problem.
                var lcs = distances[i, j, k];

                // TODO: Required to step "in"
                var topLcs = distances[i, j - 1, k];
                var leftLcs = distances[i - 1, j, k];
                var depthLcs = distances[i, j, k - 1];

                // Determine whether the token at (i, j) is a part of the LCS.
                var maxOfAny = Max(leftLcs, topLcs, depthLcs);
                var isPartOfLCS = lcs != maxOfAny;
                if (isPartOfLCS)
                {
                    // Pick the matching token.
                    Debug.Assert(first[i - 1] == second[j - 1], "first[i - 1] == second[j - 1]");
                    Debug.Assert(first[i - 1] == third[k - 1], "first[i - 1] == third[k - 1]");
                    stack.Push(first[i - 1]);

                    // Move up diagonally.
                    --i;
                    --j;
                    --k;
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
                else if (lcs == depthLcs)
                {
                    --k;
                }
            }

            return stack;
        }

        private static int[,,] InitializeLCSMatrix(ICollection first, ICollection second, ICollection third)
        {
            // Note that contrary to the Edit Distance problem, we
            // need the first row and column to be initialized with zero.
            return new int[first.Count + 1, second.Count + 1, third.Count + 1];
        }

        private static void RunMain()
        {
            InputSequence first, second, third;
            ParseInputs(out first, out second, out third);

            var solution = Solution(first, second, third);
            Console.WriteLine(solution);
        }

        private static void ParseInputs(out InputSequence first, out InputSequence second, out InputSequence third)
        {
            {
                var count = int.Parse(Console.ReadLine());
                var line = Console.ReadLine();
                first = new InputSequence(line.Split().Select(long.Parse));
                Debug.Assert(first.Count == count, "first.Count == count");
            }

            {
                var count = int.Parse(Console.ReadLine());
                var line = Console.ReadLine();
                second = new InputSequence(line.Split().Select(long.Parse));
                Debug.Assert(second.Count == count, "second.Count == count");
            }

            {
                var count = int.Parse(Console.ReadLine());
                var line = Console.ReadLine();
                third = new InputSequence(line.Split().Select(long.Parse));
                Debug.Assert(third.Count == count, "third.Count == count");
            }
        }
    }

    public sealed class InputSequence : List<long>
    {
        public InputSequence(IEnumerable<long> collection) : base(collection) { }
    }
}