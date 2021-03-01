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
	public class InstructionTests
	{
		[Test]
		public void InstructionTestSetVar()
		{
			string script = "set config/test to 5. set config/test2 to true.";
			IEnumerable<IKoalaInstruction> result = VariableParser.SetVarInstruction.Many().ParseOrThrow(script);

			List<IKoalaInstruction> instructions = result.ToList();
			Assert.IsTrue(instructions.Count == 2);
			Assert.IsInstanceOf<IKoalaInstruction>(instructions[0]);
			Assert.IsInstanceOf<IKoalaInstruction>(instructions[1]);
			Assert.IsAssignableFrom<SetVarInstruction>(instructions[0]);
			Assert.IsAssignableFrom<SetVarInstruction>(instructions[1]);

			SetVarInstruction instruction0 = instructions[0] as SetVarInstruction;
			SetVarInstruction instruction1 = instructions[1] as SetVarInstruction;

			Assert.IsNotNull(instruction0);
			Assert.IsNotNull(instruction1);

			Assert.AreEqual(new List<string> { "config", "test" }, instruction0.Path);
			Assert.AreEqual(5, instruction0.Value.AsInt32);

			Assert.AreEqual(new List<string> { "config", "test2" }, instruction1.Path);
			Assert.AreEqual(true, instruction1.Value.AsBoolean);
		}
	}

}
