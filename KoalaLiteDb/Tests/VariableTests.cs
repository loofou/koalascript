using System.Collections.Generic;
using System.Linq;
using KoalaLiteDb.Lang;
using KoalaLiteDb.Parser;
using NUnit.Framework;
using Pidgin;
using UltraLiteDB;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class VariableTests
	{
		[TestCase("hello", "hello")]
		[TestCase("hello_world", "hello_world")]
		[TestCase("hello2world", "hello2world")]
		[TestCase("hello-world", "hello-world")]
		public void TestVariableName(string script, string expected)
		{
			Assert.AreEqual(expected, VariableParser.VariableName.ParseOrThrow(script));
		}

		[TestCase("Hello")]
		[TestCase("\"world\"")]
		[TestCase("4foo")]
		[TestCase("_foo")]
		public void TestVariableNameFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.VariableName.ParseOrThrow(testScript));
		}

		[TestCase("Hello", "Hello")]
		[TestCase("Hello_world", "Hello_world")]
		[TestCase("Hello2world", "Hello2world")]
		[TestCase("Hello-world", "Hello-world")]
		public void TestCollectionName(string script, string expected)
		{
			Assert.AreEqual(expected, VariableParser.CollectionName.ParseOrThrow(script));
		}

		[TestCase("hello")]
		[TestCase("\"world\"")]
		[TestCase("4foo")]
		[TestCase("_foo")]
		public void TestCollectionNameFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.CollectionName.ParseOrThrow(testScript));
		}

		[TestCase("Hello/hello", new[] { "Hello", "hello" })]
		[TestCase("Hello    / world", new[] { "Hello", "world" })]
		[TestCase("Hello2 /   world / foo", new[] { "Hello2", "world", "foo" })]
		[TestCase("Hello-world / hello---world   / foo_baaar /\n f /\tb", new[] { "Hello-world", "hello---world", "foo_baaar", "f", "b" })]
		public void TestVariablePath(string script, string[] expected)
		{
			Assert.AreEqual(expected.ToList(), VariableParser.VariablePath.ParseOrThrow(script));
		}

		[TestCase("hello/hello")]
		[TestCase("Hello / World")]
		[TestCase("Hello /")]
		[TestCase("Hello")]
		[TestCase("world")]
		[TestCase("Hello \\ world")]
		public void TestVariablePathFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.VariablePath.ParseOrThrow(testScript));
		}

		[TestCase("set Config/testInt to 5", new[] { "Config", "testInt" }, 5, BsonType.Int32)]
		[TestCase("set Config/testBool to on", new[] { "Config", "testBool" }, true, BsonType.Boolean)]
		[TestCase("set Config/test/flag to no", new[] { "Config", "test", "flag" }, false, BsonType.Boolean)]
		[TestCase("set Config/test/string/hello-world to \"hello world\"", new[] { "Config", "test", "string", "hello-world" }, "hello world", BsonType.String)]
		public void TestSetVarInstruction(string script, string[] expectedPath, object expectedValue, BsonType expectedValueType)
		{
			IKoalaInstruction instruction = VariableParser.SetVarInstruction.ParseOrThrow(script);
			Assert.IsAssignableFrom<SetVarInstruction>(instruction);
			SetVarInstruction inst = (SetVarInstruction)instruction;

			Assert.AreEqual(expectedPath.ToList(), inst.Path);
			Assert.AreEqual(expectedValueType, inst.Value.Type);
			switch(expectedValueType)
			{
				case BsonType.Int32:
					Assert.AreEqual(expectedValue, inst.Value.AsInt32);
					break;
				case BsonType.String:
					Assert.AreEqual(expectedValue, inst.Value.AsString);
					break;
				case BsonType.Boolean:
					Assert.AreEqual(expectedValue, inst.Value.AsBoolean);
					break;
			}
		}

		[TestCase("set Config to 5")]
		[TestCase("set config/hello to 5")]
		[TestCase("Config/hello to 5")]
		[TestCase("set Config/hello 5")]
		[TestCase("set Config/hello to")]
		[TestCase("Config/hello 5")]
		[TestCase("set to")]
		public void TestSetVarInstructionFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.SetVarInstruction.ParseOrThrow(testScript));
		}
	}

}
