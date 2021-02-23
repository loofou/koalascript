using System;
using System.Collections.Generic;
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
	}

}
