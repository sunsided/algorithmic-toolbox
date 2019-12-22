using System.Diagnostics;

namespace Week5.PrimitiveCalculator
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            Debug.Assert(string.Join(" ", Program.Solution(1)) == "1", "1");
            Debug.Assert(string.Join(" ", Program.Solution(5)) == "1 2 4 5", "1 3 4 5");
            Debug.Assert(string.Join(" ", Program.Solution(96234)) == "1 3 9 10 11 22 66 198 594 1782 5346 16038 16039 32078 96234", "1 3 9 10 11 22 66 198 594 1782 5346 16038 16039 32078 96234");
        }
    }
}