using System;
using System.Diagnostics;

namespace Week6.ArithmeticExpression
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

        // Solution inspired from https://github.com/taochenshh/Maximize-the-Value-of-an-Arithmetic-Expression-with-Dynamic-Programming
        public static long Solution(string expression)
        {
            var numDigits = (expression.Length + 1) / 2;
            var values = InitializeValues(numDigits, expression);

            // Iterate through all distances between two digits.
            for (var offset = 1; offset <= numDigits; ++offset)
            {
                // Iterate through all digits, starting from the first.
                // The second digit to compare is always `offset` steps away. This way, our combinations look like:
                //     offset=1     (0,1), (1,2), (2,3), ...
                //     offset=2     (0,2), (1,3), (2,4), ...
                //     offset=3     (0,3), (1,4), (2,5), ...
                for (int i = 0, j = offset; i < numDigits - offset; ++i, ++j)
                {
                    UpdateMinMaxResultBetweenTwoDigits(i, j, values, expression);
                }
            }

            return values.Max[0, numDigits - 1];
        }

        private static void UpdateMinMaxResultBetweenTwoDigits(int i, int j, Values values, string expression)
        {
            var minVal = long.MaxValue;
            var maxVal = long.MinValue;

            // Find the minimum and maximum values for the expression between the ith and jth number.
            for (var k = i; k < j; ++k)
            {
                var operation = expression[2 * k + 1];

                var a = Evaluate(values.Min[i, k], values.Min[k + 1, j], operation);
                var b = Evaluate(values.Min[i, k], values.Max[k + 1, j], operation);
                var c = Evaluate(values.Max[i, k], values.Min[k + 1, j], operation);
                var d = Evaluate(values.Max[i, k], values.Max[k + 1, j], operation);

                minVal = Min(minVal, a, b, c, d);
                maxVal = Max(maxVal, a, b, c, d);
            }

            values.Min[i, j] = minVal;
            values.Max[i, j] = maxVal;
        }

        private static Values InitializeValues(int numDigits, string expression)
        {
            var values = new Values(numDigits);

            // Initialize the main diagonals with the digits themselves.
            for (var i = 0; i < numDigits; i++)
            {
                var digit = long.Parse(expression.Substring(2 * i, 1));
                values.Min[i, i] = values.Max[i, i] = digit;
            }

            return values;
        }

        private static long Min(long a, long b, long c, long d, long e)
        {
            return Math.Min(a, Math.Min(b, Math.Min(c, Math.Min(d, e))));
        }

        private static long Max(long a, long b, long c, long d, long e)
        {
            return Math.Max(a, Math.Max(b, Math.Max(c, Math.Max(d, e))));
        }

        private static long Evaluate(long a, long b, char op)
        {
            switch (op)
            {
                case '*': return a * b;
                case '+': return a + b;
                case '-': return a - b;
                default: throw new InvalidOperationException("This should never happen.");
            }
        }

        private static void RunMain()
        {
            var expression = ParseInputs();

            var solution = Solution(expression);
            Console.WriteLine(solution);
        }

        private static string ParseInputs()
        {
            // The only line of the input contains a string s of length 2n + 1 for some n, with symbols
            // s(0), s(1), ..., s(2n). Each symbol at an even position of s is a digit (that is,
            // an integer from 0 to 9) while  each symbol at an odd position is one of three operations from {+,-,*}.
            // Constraint: 1 ≤ n ≤ 14 (hence the string contains at most 29 symbols).
            var line = Console.ReadLine().Trim();
            Debug.Assert(line.Length <= 29, "line.Length <= 29");
            return line;
        }

        private sealed class Values
        {
            public Values(int numDigits)
            {
                Min = new long[numDigits, numDigits];
                Max = new long[numDigits, numDigits];
            }

            public long[,] Min { get; }
            public long[,] Max { get; }
        }
    }
}