using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;
using UltraLiteDB;

namespace KoalaLiteDb.Parser
{

	public class KoalaInterpreter
	{
		readonly UltraLiteDatabase database;

		public KoalaInterpreter(UltraLiteDatabase database)
		{
			this.database = database;
		}

		public void RunScript(string script)
		{
			IEnumerable<InitCollectionInstruction> instructions = KoalaParser.MainParser.ParseOrThrow(script);

			foreach(InitCollectionInstruction instruction in instructions)
			{
				instruction.Run(database);
			}
		}
	}

}
