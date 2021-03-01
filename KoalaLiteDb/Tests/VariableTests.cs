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

		[TestCase("hello", new[] { "hello" })]
		[TestCase("hello    / world", new[] { "hello", "world" })]
		[TestCase("hello2 /   world / foo", new[] { "hello2", "world", "foo" })]
		[TestCase("hello-world / hello---world   / foo_baaar /\n f /\tb", new[] { "hello-world", "hello---world", "foo_baaar", "f", "b" })]
		public void TestVariablePath(string script, string[] expected)
		{
			Assert.AreEqual(expected.ToList(), VariableParser.VariablePath.ParseOrThrow(script));
		}

		[TestCase("Hello/hello")]
		[TestCase("hello / World")]
		[TestCase("hello /")]
		[TestCase("World")]
		public void TestVariablePathFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.VariablePath.ParseOrThrow(testScript));
		}

		[TestCase("set config/testInt to 5.", new[] { "config", "testInt" }, 5, BsonType.Int32)]
		[TestCase("set config/testBool to on.", new[] { "config", "testBool" }, true, BsonType.Boolean)]
		[TestCase("set config/test/flag to no.", new[] { "config", "test", "flag" }, false, BsonType.Boolean)]
		[TestCase("set config/test/string/hello-world to \"hello world\".", new[] { "config", "test", "string", "hello-world" }, "hello world"
				, BsonType.String)]
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

		[TestCase("set Config to 5.")]
		[TestCase("set config/hello to 5")]
		[TestCase("config/hello to 5.")]
		[TestCase("set config/hello 5.")]
		[TestCase("set config/hello to.")]
		[TestCase("config/hello 5.")]
		[TestCase("set to.")]
		public void TestSetVarInstructionFail(string testScript)
		{
			Assert.Throws<ParseException>(() => VariableParser.SetVarInstruction.ParseOrThrow(testScript));
		}
	}

}
