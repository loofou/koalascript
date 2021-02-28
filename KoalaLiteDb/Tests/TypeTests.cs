using KoalaLiteDb.Parser;
using NUnit.Framework;
using Pidgin;
using UltraLiteDB;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class TypeTests
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
		public void TestBool(string testScript, bool expected)
		{
			Assert.AreEqual(expected, TypeParser.Bool.ParseOrThrow(testScript));
		}

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
		public void TestBoolType(string testScript, bool expected)
		{
			BsonValue bsonValue = TypeParser.BoolType.ParseOrThrow(testScript);
			Assert.IsTrue(bsonValue.IsBoolean);
			Assert.AreEqual(expected, bsonValue.AsBoolean);
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
		public void TestBoolFail(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.Bool.ParseOrThrow(testScript));
		}

		[TestCase("5", 5)]
		[TestCase("0", 0)]
		[TestCase("1564651615", 1564651615)]
		[TestCase("-3", -3)]
		[TestCase("+15", 15)]
		public void TestNumber(string testScript, int expected)
		{
			Assert.AreEqual(expected, TypeParser.Number.ParseOrThrow(testScript));
		}

		[TestCase("5", 5)]
		[TestCase("0", 0)]
		[TestCase("1564651615", 1564651615)]
		[TestCase("-3", -3)]
		[TestCase("+15", 15)]
		public void TestNumberType(string testScript, int expected)
		{
			BsonValue bsonValue = TypeParser.NumberType.ParseOrThrow(testScript);
			Assert.IsTrue(bsonValue.IsNumber);
			Assert.AreEqual(expected, bsonValue.AsInt32);
		}

		[TestCase("--5")]
		[TestCase("++++5")]
		[TestCase(".5")]
		[TestCase("peter")]
		public void TestNumberFail(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.Number.ParseOrThrow(testScript));
		}

		[TestCase("\"hello\"", "hello")]
		[TestCase("\"world\"", "world")]
		[TestCase("\"true thing\"", "true thing")]
		[TestCase("\"15\"", "15")]
		[TestCase("\"\"", "")]
		[TestCase("\"\"\"", "")]
		public void TestString(string testScript, string expected)
		{
			Assert.AreEqual(expected, TypeParser.String.ParseOrThrow(testScript));
		}

		[TestCase("\"hello\"", "hello")]
		[TestCase("\"world\"", "world")]
		[TestCase("\"true thing\"", "true thing")]
		[TestCase("\"15\"", "15")]
		[TestCase("\"\"", "")]
		[TestCase("\"\"\"", "")]
		public void TestStringType(string testScript, string expected)
		{
			BsonValue bsonValue = TypeParser.StringType.ParseOrThrow(testScript);
			Assert.IsTrue(bsonValue.IsString);
			Assert.AreEqual(expected, bsonValue.AsString);
		}

		[TestCase("hello")]
		[TestCase("\"world")]
		[TestCase("foo\"")]
		public void TestStringFail(string testScript)
		{
			Assert.Throws<ParseException>(() => TypeParser.String.ParseOrThrow(testScript));
		}
	}

}
