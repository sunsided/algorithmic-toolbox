using System.Collections.Generic;
using System.Diagnostics;

namespace Week6.ArithmeticExpression
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            foreach (var testCase in GenerateTestCases())
            {
                var outcome = Program.Solution(testCase.Equation);
                Debug.Assert(outcome == testCase.ExpectedOutcome,
                    $"Result {outcome} != {testCase.ExpectedOutcome} for {testCase.Equation}");
            }
        }

        private static IEnumerable<Test> GenerateTestCases()
        {
            // trivial
            yield return GenerateTestCase(6, "1+5");

            // 200 = (5 − ((8 + 7) × (4 − (8 + 9))))
            yield return GenerateTestCase(200, "5-8+7*4-8+9");
        }

        private static Test GenerateTestCase(long expectedOutcome, string equation)
        {
            return new Test(expectedOutcome, equation);
        }

        private struct Test
        {
            public readonly long ExpectedOutcome;
            public readonly string Equation;

            public Test(long expectedOutcome, string equation)
            {
                ExpectedOutcome = expectedOutcome;
                Equation = equation;
            }
        }
    }
}