using System.Diagnostics;

namespace Week5.EditDistance
{
    internal static class Testing
    {
        [Conditional("TESTING")]
        public static void Run()
        {
            Debug.Assert(Program.Solution("editing", "distance") == 5, "|editing -> distance| == 5");
            Debug.Assert(Program.Solution("a", "a") == 0, "|a -> a| == 0");
            Debug.Assert(Program.Solution("a", "b") == 1, "|a -> b| == 1");
            Debug.Assert(Program.Solution("ab", "ab") == 0, "|ab -> ab| == 0");
            Debug.Assert(Program.Solution("ab", "abc") == 1, "|ab -> abc| == 1");
            Debug.Assert(Program.Solution("abc", "ab") == 1, "|abc -> ab| == 1");
            Debug.Assert(Program.Solution("back", "book") == 2, "|back -> book| == 2");
            Debug.Assert(Program.Solution("back", "books") == 3, "|back -> books| == 3");
        }
    }
}