using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Func.Net.Tests
{
    [TestClass]
    public class TryTests
    {
        object a = new object();
        object b = new object();

        [TestMethod]
        public void Sanity()
        {
            ITry<object> t = Try.Of(() => checkedMethod(true, a));
            Assert.AreEqual(true, t.IsSuccess);
            Assert.AreEqual(a, t.Get());

            ITry<object> t2 = Try.Of(() => checkedMethod(false, a));
            Assert.AreEqual(true, t2.IsFailure);
            Assert.ThrowsException<Exception>(()=> t2.Get());
        }

        [TestMethod]
        public void RunTest()
        {
            Assert.AreEqual(true, Try.Run(() => checkedMethod(false, a)).IsFailure);
        }

        [TestMethod]
        public void OrElseTest()
        {
            Assert.AreEqual(a, Try.Of(() => checkedMethod(true, a)).OrElse(b));
            Assert.AreEqual(b, Try.Of(() => checkedMethod(false, a)).OrElse(b));
        }

        private object checkedMethod(bool shouldPass, object returnVal)
        {
            if (!shouldPass)
                throw new Exception("");

            return returnVal ?? new object();
        }
    }
}
