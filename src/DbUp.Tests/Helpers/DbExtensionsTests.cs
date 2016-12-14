using DbUp.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbUp.Tests.Helpers
{
    [TestFixture]
    public class DbExtensionsTests
    {
        [Test]
        public void should_returns_null_if_input_is_null()
        {
            object obj = null;
            var result = obj.Get<string>();

            Assert.AreEqual(null, result);
        }

        [Test]
        public void should_returns_correct_string_if_input_is_string()
        {
            object obj = "test_string";
            var result = obj.Get<string>();

            Assert.AreEqual("test_string", result);
        }
    }
}
