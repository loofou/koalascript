using System.Data;
using System.IO;
using KoalaLiteDb.Interpreter;
using KoalaLiteDb.Parser;
using NUnit.Framework;
using UltraLiteDB;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class InterpreterTests
	{
		[SetUp]
		public void SetUp()
		{
			File.Delete("InterpreterTests.db");
		}

		[Test]
		public void SetVarInstructionFail()
		{
			string script = @"init Configs:
	make testConfig (
		set tag to ""hello world"".
		set tag / test2 to on.
		)
		end";

			using KoalaDatabase database = new();
			KoalaInterpreter interpreter = new(database);

			DataException ex = Assert.Throws<DataException>(() => interpreter.RunScript(script));
			Assert.That(ex?.Message, Is.EqualTo("Can't set variable at 'tag/test2'! 'tag' is not a document!"));
		}
	}

}
