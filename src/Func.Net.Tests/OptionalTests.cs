using System;
using System.Collections.Generic;

using Func.Net.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Func.Net.Tests
{
    [TestClass]
    public class OptionalTests
    {
        private void checkEmpty(Optional<string> empty)
        {
            Assert.IsTrue(empty.Equals(Optional.Empty<string>()));
            Assert.IsFalse(empty.IsPresent);
            Assert.IsTrue(empty.IsEmpty);
            Assert.AreEqual(empty.GetHashCode(),     0);
            Assert.AreEqual(empty.OrElse("x"),       "x");
            Assert.AreEqual(empty.OrElse(() => "y"), "y");
            Assert.ThrowsException<ArgumentNullException>(empty.Get);
            Assert.ThrowsException<ArgumentNullException>(() => empty.OrElseThrow(() => new ArgumentNullException()));

            bool b = false;

            empty.IfPresent(s => b = true);
            Assert.IsFalse(b);

            bool b1 = false;
            bool b2 = false;

            empty.Match(s => b1 = true, () => b2 = true);
            Assert.IsFalse(b1);
            Assert.IsTrue(b2);

            Assert.AreEqual("empty", empty.Match(e => "unexpected", () => "empty"));

            Assert.AreEqual("Optional[Empty]", empty.ToString());
        }

        private void checkExists(Optional<string> opt, string expected)
        {
            Assert.IsFalse(opt.Equals(Optional.Empty<string>()));
            Assert.IsTrue(opt.IsPresent);
            Assert.IsFalse(opt.IsEmpty);
            Assert.AreEqual(Optional.Of(expected), opt);
            Assert.AreNotEqual(Optional.Of("unexpected"), opt);
            Assert.AreEqual(expected.GetHashCode(), opt.GetHashCode());
            Assert.AreEqual(expected,               opt.OrElse("unexpected"));
            Assert.AreEqual(expected,               opt.OrElse(() => "unexpected"));
            Assert.AreEqual(expected,               opt.Get());
            Assert.AreEqual(expected,               opt.OrElseThrow(() => new ArgumentNullException()));

            bool b = false;
            opt.IfPresent(s => b = true);
            Assert.IsTrue(b);

            bool b1 = false;
            bool b2 = false;

            opt.Match(s => b1 = true, () => b2 = true);
            Assert.IsTrue(b1);
            Assert.IsFalse(b2);

            Assert.AreEqual(expected, opt.Match(e => e, () => "empty"));

            Assert.AreEqual("Optional[" + expected + "]", opt.ToString());
        }

        [TestMethod]
        public void Sanity()
        {
            checkEmpty(Optional.Empty<string>());
            Assert.ThrowsException<NullReferenceException>(() => Optional.Of((string)null));
            checkEmpty(Optional.OfNullable((string)null));
            checkExists(Optional.Empty<string>().OrOptional("foo"),       "foo");
            checkExists(Optional.Empty<string>().OrOptional(() => "foo"), "foo");
            checkExists(Optional.Of("foo").OrOptional(() => { Assert.Fail(); return "bar"; }), "foo");
        }

        [TestMethod]
        public void DictionaryExtensionTest()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "One");
            dict.Add(2, "Two");
            dict.Add(3, "Three");
            dict.Add(4, null);

            checkExists(dict.GetOptional(1), "One");
            checkEmpty(dict.GetOptional(5));
            checkEmpty(dict.GetOptional(4));
        }

        [TestMethod]
        public void SetExtensionTest()
        {
            HashSet<string> set = new HashSet<string>();
            set.Add("Foo");
            set.Add("Bar");

            checkExists(set.GetOptional("Foo"), "Foo");
            checkEmpty(set.GetOptional("foobar"));
        }
    }
}