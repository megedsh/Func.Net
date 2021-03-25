using System;
using System.Collections.Generic;
using System.Text;

using Func.Net.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Func.Net.Tests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1,"One");
            dict.Add(2, "Two");
            dict.Add(3, "Three");
            dict.Add(4, null);

            Assert.AreEqual("One", dict.GetOptional(1).Get());
            Assert.IsTrue(dict.GetOptional(5).IsEmpty);
            Assert.IsTrue(dict.GetOptional(4).IsEmpty);
        }

        [TestMethod]
        public void TestMethod2()
        {
            HashSet<string> set = new HashSet<string>();
            set.Add("Foo");
            set.Add("Bar");

            set.GetOptional("Foo").Match( (v)=>Assert.AreEqual("Foo",v),Assert.Fail);

            set.GetOptional("foobar").IfPresent( (v)=>Assert.Fail());
        }



    }
}
