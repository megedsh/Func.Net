using System.Runtime.InteropServices.ComTypes;

using Func.Net.Match;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Func.Net.Match.MatchApi;
using Case = Func.Net.Match.Case;

namespace Func.Net.Tests
{
    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void TestMethod1()
        {

            ICase<int, string>[] cases = Case.ArrayOf(Case(1, "One"),
                                                      Case(2, "Two"),
                                                      Case(Any<int>(), "?"));

            string m = 5.Match().Of(cases);
            string of = 2.Match().Of(cases);
        }
    }
}
