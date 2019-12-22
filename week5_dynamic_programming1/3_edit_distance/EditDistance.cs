using System;

namespace Week5.EditDistance
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

        public static int Solution(string source, string target)
        {
            var distances = InitializeDistanceMatrix(source, target);

            for (var j = 1; j <= target.Length; ++j)
            {
                for (var i = 1; i <= source.Length; ++i)
                {
                    // Determine editing costs.
                    var insertion = distances[i, j - 1] + 1;
                    var deletion = distances[i - 1, j] + 1;
                    var match = distances[i - 1, j - 1];
                    var mismatch = match + 1;

                    // Select minimum cost depending on the case.
                    var charactersMatch = source[i - 1] == target[j - 1];
                    distances[i, j] = charactersMatch
                        ? Min(insertion, deletion, match)
                        : Min(insertion, deletion, mismatch);
                }
            }

            return distances[source.Length, target.Length];
        }

        private static int[,] InitializeDistanceMatrix(string source, string target)
        {
            // Allocate the distance matrix and initialize with 0, 1, 2, ... along the zeroth row and column.
            var distances = new int[source.Length + 1, target.Length + 1];
            for (var i = 0; i < distances.GetLength(0); ++i) distances[i, 0] = i;
            for (var j = 0; j < distances.GetLength(1); ++j) distances[0, j] = j;
            return distances;
        }

        private static int Min(int a, int b, int c)
        {
            return Math.Min(Math.Min(a, b), c);
        }

        private static void RunMain()
        {
            string source, target;
            ParseInputs(out source, out target);

            var solution = Solution(source, target);
            Console.WriteLine(solution);
        }

        private static void ParseInputs(out string source, out string target)
        {
            source = Console.ReadLine();
            target = Console.ReadLine();
        }
    }
}