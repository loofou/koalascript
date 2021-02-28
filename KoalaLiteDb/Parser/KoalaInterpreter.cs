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
			IEnumerable<IKoalaInstruction> instructions = KoalaParser.MainParser.ParseOrThrow(script);

			foreach(IKoalaInstruction instruction in instructions)
			{
				//instruction.Run(database);
			}
		}
	}

}
