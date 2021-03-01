using System.Collections.Generic;
using System.Linq;
using KoalaLiteDb.Lang;
using KoalaLiteDb.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class DatasetTests
	{
		[Test]
		public void MakeInstructionTest()
		{
			string script = "make configTest (set config/test to 5. set config/test2 to true.)";
			IEnumerable<MakeDatasetInstruction> result = DatasetParser.MakeInstruction.Many().ParseOrThrow(script);

			List<MakeDatasetInstruction> makeDatasetInstructions = result.ToList();
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
