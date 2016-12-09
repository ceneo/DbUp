using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbUp.Engine;
using DbUp.Tests.TestInfrastructure;
using NSubstitute;
using NUnit.Framework;

namespace DbUp.Tests.Engine
{
    [TestFixture]
    public class LazySqlScriptTests
    {
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            hasher = Substitute.For<IHasher>();
        }

        [Test]
        public void returns_hash_from_name()
        {
            const string scriptName = "00001_Script1";
            const string expectedHash = "ahffa52162121ASAS";
            hasher.GenerateHash(scriptName).Returns(expectedHash);
            var lazySqlScript = new LazySqlScript(scriptName, ()=>"", hasher);

            Assert.AreEqual(expectedHash, lazySqlScript.Hash);
        }
    }
}
