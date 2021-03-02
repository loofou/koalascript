using System.Collections.Generic;
using KoalaLiteDb.Lang;
using KoalaLiteDb.Parser;
using Pidgin;
using UltraLiteDB;

namespace KoalaLiteDb.Interpreter
{

	public class KoalaInterpreter
	{
		readonly KoalaDatabase database;

		public KoalaInterpreter(KoalaDatabase database)
		{
			this.database = database;
		}

		public void RunScript(string script)
		{
			IEnumerable<InitCollectionInstruction> instructions = KoalaParser.MainParser.ParseOrThrow(script);

			foreach(InitCollectionInstruction instruction in instructions)
			{
				instruction.Run(database.Database);
			}
		}
	}

}
