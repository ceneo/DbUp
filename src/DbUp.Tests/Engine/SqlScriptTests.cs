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
    public class SqlScriptTests
    {
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            hasher = Substitute.For<IHasher>();
        }

        [Test]
        public void returns_hash_from_name_and_content()
        {
            const string scriptName = "00001_Script1";
            const string content = "SELECT 1 FROM db.test";
            const string expectedHash = "ahffa52162121ASAS";
            hasher.GenerateHash(scriptName + content).Returns(expectedHash);
            var sqlScript = new SqlScript(scriptName, content, hasher);

            Assert.AreEqual(expectedHash, sqlScript.Hash);
        }

        [Test]
        public void match_to_returns_false_if_executed_script_has_different_name()
        {
            var sqlScript = new SqlScript("00001_Script1", "SELECT 1 FROM dbo.test", hasher);
            var executedScript = new ExecutedSqlScript("00001_Script2", "24343jjj324234");

            Assert.False(sqlScript.MatchTo(executedScript));
        }

        [TestCase(null)]
        [TestCase("")]
        public void match_to_returns_true_if_executed_script_has_the_same_name_and_executed_script_hash_is_null_or_empty(string executedScriptHash)
        {
            var sqlScript = new SqlScript("00001_Script1", "SELECT 1 FROM dbo.test", hasher);
            var executedScript = new ExecutedSqlScript("00001_Script1", executedScriptHash);

            Assert.True(sqlScript.MatchTo(executedScript));
        }

        [Test]
        public void match_to_throws_exception_if_executed_script_has_the_same_name_and_executed_script_hash_is_not_verified_by_hasher()
        {
            const string executedScriptHash = "24343jjj324234";
            hasher.Verify(executedScriptHash).Returns(x => { throw new Exception();});
            var sqlScript = new SqlScript("00001_Script1", "SELECT 1 FROM dbo.test", hasher);
            var executedScript = new ExecutedSqlScript("00001_Script1", executedScriptHash);

            Assert.Throws<Exception>(()=>sqlScript.MatchTo(executedScript));
        }

        [Test]
        public void match_to_returns_true_if_executed_script_has_the_same_name_and_executed_script_has_the_same_hash()
        {
            const string executedScriptHash = "24343jjj324234";
            hasher.Verify(executedScriptHash).Returns(true);
            hasher.GenerateHash(Arg.Any<string>()).Returns(executedScriptHash);
            var sqlScript = new SqlScript("00001_Script1", "SELECT 1 FROM dbo.test", hasher);
            var executedScript = new ExecutedSqlScript("00001_Script1", executedScriptHash);

            Assert.True(sqlScript.MatchTo(executedScript));
        }

        [Test]
        public void match_to_returns_false_if_executed_script_has_the_same_name_and_executed_script_has_different_hash()
        {
            const string executedScriptHash = "24343jjj324234";
            hasher.Verify(executedScriptHash).Returns(true);
            hasher.GenerateHash(Arg.Any<string>()).Returns("1343dlfjdf3434");
            var sqlScript = new SqlScript("00001_Script1", "SELECT 1 FROM dbo.test", hasher);
            var executedScript = new ExecutedSqlScript("00001_Script1", executedScriptHash);

            Assert.False(sqlScript.MatchTo(executedScript));
        }

    }
}
