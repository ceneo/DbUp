using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbUp.Engine.Hashers;
using NUnit.Framework;

namespace DbUp.Tests.Engine
{
    [TestFixture]
    public class Md5WithMarkHasherTests
    {
        private Md5WithMarkHasher hasher;

        [SetUp]
        public void SetUp()
        {
            hasher = new Md5WithMarkHasher();
        }

        [Test]
        public void generates_md5_hash_with_mark_at_the_beginning()
        {
            var input = "0001_Scrip1Select 1 FROM dbo.test";
            var hash = hasher.GenerateHash(input);

            Assert.AreEqual("MD5alg_8/Q6OJzAi9EXPq3LYgqnXw==", hash);
        }

        [Test]
        public void not_verifies_if_hash_not_begins_with_MD5alg_mark()
        {
            var verified = hasher.Verify("3945dsf8sdf7sdfsd");
            Assert.False(verified);
        }

        [Test]
        public void verifies_if_hash_begins_with_MD5alg_mark()
        {
            var verified = hasher.Verify("MD5alg_3945dsf8sdf7sdfsd");
            Assert.True(verified);
        }
    }
}
