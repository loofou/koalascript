using KoalaLiteDb.Lang;
using Pidgin;
using static Pidgin.Parser;
using static KoalaLiteDb.Parser.ParserHelper;
using static KoalaLiteDb.Parser.VariableParser;

namespace KoalaLiteDb.Parser
{

	internal static class DatasetParser
	{
		static readonly Parser<char, char> ParenthesisOpen = Tok('(');
		static readonly Parser<char, char> ParenthesisClose = Tok(')');
		static readonly Parser<char, string> Make = Tok("make");
		static readonly Parser<char, string> Optimize = Tok("optimize");
		static readonly Parser<char, string> Unique = Tok("unique");

		internal static readonly Parser<char, MakeDatasetInstruction> MakeInstruction =
			Map((datasetName, instructions) => new MakeDatasetInstruction(datasetName, instructions)
			  , Line(Make
					.Then(VariableName)
					.Before(ParenthesisOpen))
			  , Line(VariableParser.SetVarInstruction).Many())
			   .Before(Line(ParenthesisClose));

		internal static readonly Parser<char, EnsureIndexInstruction> OptimizeInstruction =
			Line(Map((_, unique, path) => new EnsureIndexInstruction(path, unique.HasValue)
				   , Optimize
				   , Unique.Optional()
				   , VariablePath).Before(Dot));
	}

}
