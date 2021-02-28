using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;
using UltraLiteDB;
using static KoalaLiteDb.Parser.InstructionParser;

namespace KoalaLiteDb.Parser
{

	public static class KoalaParser
	{
		public static readonly Parser<char, IEnumerable<IKoalaInstruction>> MainParser = InstructionLines;
	}

}
