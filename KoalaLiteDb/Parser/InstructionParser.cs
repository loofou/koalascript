using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;
using UltraLiteDB;
using static Pidgin.Parser;
using static KoalaLiteDb.Parser.ParserHelper;

namespace KoalaLiteDb.Parser
{

	internal static class InstructionParser
	{
		static readonly Parser<char, IKoalaInstruction> Instruction =
			Try(OneOf(
					  VariableParser.SetVarInstruction
					 ));

		internal static readonly Parser<char, IEnumerable<IKoalaInstruction>> InstructionLines =
			Line(Instruction).Many();
	}

}
