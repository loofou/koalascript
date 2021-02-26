using System.Collections.Generic;
using KoalaScript.Lang;
using KoalaScript.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaTest
{

	[TestFixture]
	public class KoalaTypeParserTests
	{
		[TestCase("true", true)]
		[TestCase("on", true)]
		[TestCase("yes", true)]
		[TestCase("True", true)]
		[TestCase("On", true)]
		[TestCase("Yes", true)]
		[TestCase("false", false)]
		[TestCase("off", false)]
		[TestCase("no", false)]
		[TestCase("False", false)]
		[TestCase("Off", false)]
		[TestCase("No", false)]
		public void TestBools(string testScript, bool expected)
		{
			Assert.AreEqual(expected, TypeParser.Bool.ParseOrThrow(testScript));
		}

		[TestCase("TRUE")]
		[TestCase("FALSE")]
		[TestCase("ON")]
		[TestCase("OFF")]
		[TestCase("YES")]
		[TestCase("NO")]
		[TestCase("peter")]
		[TestCase("trve")]
		[TestCase("1")]
		[TestCase("0")]
		[TestCase("5")]
		public void TestBools(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.Bool.ParseOrThrow(testScript));
		}
		
		[TestCase("5", 5)]
		[TestCase("0", 0)]
		[TestCase("1564651615", 1564651615)]
		[TestCase("-3", -3)]
		[TestCase("+15", 15)]
		public void TestNumbers(string testScript, int expected)
		{
			Assert.AreEqual(expected, TypeParser.Number.ParseOrThrow(testScript));
		}

		[TestCase("--5")]
		[TestCase("++++5")]
		[TestCase(".5")]
		[TestCase("peter")]
		public void TestNumbers(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.Number.ParseOrThrow(testScript));
		}
		
		[TestCase("\"hello\"", "hello")]
		[TestCase("\"world\"", "world")]
		[TestCase("\"true thing\"", "true thing")]
		[TestCase("\"15\"", "15")]
		[TestCase("\"\"", "")]
		[TestCase("\"\"\"", "")]
		public void TestStrings(string testScript, string expected)
		{
			Assert.AreEqual(expected, TypeParser.String.ParseOrThrow(testScript));
		}

		[TestCase("hello")]
		[TestCase("\"world")]
		[TestCase("foo\"")]
		public void TestStrings(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.String.ParseOrThrow(testScript));
		}
		
		[TestCase("hello", "hello")]
		[TestCase("hello_world", "hello_world")]
		[TestCase("hello2world", "hello2world")]
		[TestCase("hello-world", "hello-world")]
		public void TestVariables(string testScript, string expected)
		{
			Assert.AreEqual(expected, TypeParser.VariableName.ParseOrThrow(testScript));
		}

		[TestCase("Hello")]
		[TestCase("\"world\"")]
		[TestCase("4foo")]
		[TestCase("_foo")]
		public void TestVariables(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.VariableName.ParseOrThrow(testScript));
		}

		[TestCase("set testString to \"hello world\".", "testString", "hello world")]
		[TestCase("set testNumber to 5.", "testNumber", 5)]
		[TestCase("set testBool to on.", "testBool", true)]
		[TestCase("set testVar to testOther.", "testVar", "testOther")]
		public void TestTypeDefinitions(string testScript, string expectedVarName, object expectedValue)
		{
			KeyValuePair<KVar,KoalaType> pair = GenericParser.TypeDefinition.ParseOrThrow(testScript);
			Assert.AreEqual(expectedVarName, pair.Key);
			Assert.AreEqual(expectedValue, pair.Value);
		}
		
		[TestCase("set testString to hello world.")]
		[TestCase("set TestString to \"hello world\".")]
		[TestCase("set testString to \"\"hello world\".")]
		[TestCase("set testString to \"hello world\"\".")]
		[TestCase("set testString to \"hello world\"")]
		[TestCase("set test to Test1.")]
		[TestCase("set test test1.")]
		[TestCase("test to on.")]
		[TestCase("test on.")]
		[TestCase("on.")]
		[TestCase("on")]
		[TestCase("")]
		public void TestTypeDefinitions(string testScript)
		{
			Assert.Throws<ParseException>(() => GenericParser.TypeDefinition.ParseOrThrow(testScript));
		}
	}

}
