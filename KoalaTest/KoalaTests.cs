using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KoalaScript.Lang;
using KoalaScript.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaTest
{

	[TestFixture]
	public class Tests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void TestKoalaScriptFile()
		{
			string scriptFile = File.ReadAllText("../../../../KoalaScript/test.koala");
			Result<char, IEnumerable<KeyValuePair<KVar, KoalaType>>> result = KoalaInterpreter.Run(scriptFile);
			Console.WriteLine(string.Join("\n", result.Value.Select(p => p)));
			Assert.Pass();
		}

		[TestCase("set $testVar to 5", "testVar", VariableScope.Local, 5)]
		[TestCase("set $test to 5.5457", "test", VariableScope.Local, 5.5457)]
		[TestCase("set %t to 5.3e5", "t", VariableScope.Global, 5.3e5)]
		[TestCase("set ~dar to 11", "dar", VariableScope.Temp, 11)]
		public void TestNumbers(string testScript, string expectedVarName, VariableScope expectedScope, double expectedValue)
		{
			KeyValuePair<KVar, KoalaType> type = KoalaParser.TypeDefinition.ParseOrThrow(testScript);

			Assert.IsTrue(type.Key.RawValue == expectedVarName);
			Assert.IsTrue(type.Key.Scope == expectedScope);
			Assert.IsTrue(type.Value is KNumber);
			Assert.IsTrue(Math.Abs((KNumber)type.Value - expectedValue) < 0.0001);
		}

		[TestCase("set $testVar to off", "testVar", VariableScope.Local, false)]
		[TestCase("set $test to on", "test", VariableScope.Local, true)]
		[TestCase("set %t to True", "t", VariableScope.Global, true)]
		[TestCase("set ~dar to false", "dar", VariableScope.Temp, false)]
		[TestCase("set $testVar to yes", "testVar", VariableScope.Local, true)]
		[TestCase("set $testVar to No", "testVar", VariableScope.Local, false)]
		public void TestBools(string testScript, string expectedVarName, VariableScope expectedScope, bool expectedValue)
		{
			KeyValuePair<KVar, KoalaType> type = KoalaParser.TypeDefinition.ParseOrThrow(testScript);

			Assert.IsTrue(type.Key.RawValue == expectedVarName);
			Assert.IsTrue(type.Key.Scope == expectedScope);
			Assert.IsTrue(type.Value is KBool);
			Assert.IsTrue((KBool)type.Value == expectedValue);
		}

		[TestCase("set")]
		[TestCase("set testVar")]
		[TestCase("set $testVar hello")]
		[TestCase("set $testVar to")]
		[TestCase("set $Testvar to 5")]
		[TestCase("set $testVar to orange 5")]
		[TestCase("set $testVar to hello world")]
		public void TestFails(string testScript)
		{
			Assert.Catch<ParseException>(() => KoalaParser.TypeDefinition.ParseOrThrow(testScript));
		}
	}

}
