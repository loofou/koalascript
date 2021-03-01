using System.Collections.Generic;
using System.Linq;
using KoalaLiteDb.Lang;
using KoalaLiteDb.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class CollectionTests
	{
		[TestCase("Hello", "Hello")]
		[TestCase("Hello_world", "Hello_world")]
		[TestCase("Hello2world", "Hello2world")]
		[TestCase("Hello-world", "Hello-world")]
		public void TestCollectionName(string script, string expected)
		{
			Assert.AreEqual(expected, CollectionParser.CollectionName.ParseOrThrow(script));
		}

		[TestCase("hello")]
		[TestCase("\"world\"")]
		[TestCase("4foo")]
		[TestCase("_foo")]
		public void TestCollectionNameFail(string testScript)
		{
			Assert.Throws<ParseException>(() => CollectionParser.CollectionName.ParseOrThrow(testScript));
		}

		[TestCase("init Hello:", "Hello")]
		[TestCase("init Hello_world   :  ", "Hello_world")]
		[TestCase("init \n Hello2world\t\n\t:", "Hello2world")]
		[TestCase("init   Hello-world :", "Hello-world")]
		public void TestInitInstruction(string script, string expected)
		{
			Assert.AreEqual(expected, CollectionParser.InitLine.ParseOrThrow(script));
		}

		[TestCase("init hello:")]
		[TestCase("init \"world\" :")]
		[TestCase("init Hoo")]
		[TestCase("Foo:")]
		public void TestInitInstructionFail(string testScript)
		{
			Assert.Throws<ParseException>(() => CollectionParser.InitLine.ParseOrThrow(testScript));
		}

		[Test]
		public void InitInstructionTest()
		{
			string script = "init Config: make configTest (set config/test to 5. set config/test2 to true.) end";
			IEnumerable<InitCollectionInstruction> result = CollectionParser.InitInstruction.Many().ParseOrThrow(script);

			List<InitCollectionInstruction> initInstructions = result.ToList();
			Assert.IsTrue(initInstructions.Count == 1);

			InitCollectionInstruction init = initInstructions[0];
			Assert.AreEqual("Config", init.CollectionName);

			List<MakeDatasetInstruction> makeDatasetInstructions = init.MakeDatasetInstructions.ToList();
			Assert.IsTrue(makeDatasetInstructions.Count == 1);

			MakeDatasetInstruction make = makeDatasetInstructions[0];
			Assert.AreEqual("configTest", make.DatasetName.AsString);

			List<SetVarInstruction> instructions = make.Instructions.ToList();

			SetVarInstruction instruction0 = instructions[0];
			SetVarInstruction instruction1 = instructions[1];

			Assert.IsNotNull(instruction0);
			Assert.IsNotNull(instruction1);

			Assert.AreEqual(new List<string> { "config", "test" }, instruction0.Path);
			Assert.AreEqual(5, instruction0.Value.AsInt32);

			Assert.AreEqual(new List<string> { "config", "test2" }, instruction1.Path);
			Assert.AreEqual(true, instruction1.Value.AsBoolean);
		}
	}

}
