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
	}

}
