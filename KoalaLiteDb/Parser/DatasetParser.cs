using KoalaLiteDb.Lang;
using Pidgin;
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
			Tok(
				Make
				   .Then(VariableName)
				   .Before(ParenthesisOpen)
				   .SelectMany(_ => VariableParser.SetVarInstruction.Many(),
							   (datasetName, instructions) =>
								   new MakeDatasetInstruction(datasetName, instructions))
				   .Before(ParenthesisClose)
			   );

		internal static readonly Parser<char, EnsureIndexInstruction> OptimizeInstruction =
			Tok(
				Optimize
				   //.Then(Unique).Optional()
				   .SelectMany(_ => VariablePath
							 , (unique, path) => new EnsureIndexInstruction(path,/* unique.HasValue*/ false))
				   .Before(Dot)
			   );
	}

}
