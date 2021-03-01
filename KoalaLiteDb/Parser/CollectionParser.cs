using KoalaLiteDb.Lang;
using Pidgin;
using static KoalaLiteDb.Parser.DatasetParser;
using static Pidgin.Parser;
using static KoalaLiteDb.Parser.ParserHelper;

namespace KoalaLiteDb.Parser
{

	internal static class CollectionParser
	{
		static readonly Parser<char, char> Colon = Tok(':');
		static readonly Parser<char, string> Init = Tok("init");
		static readonly Parser<char, string> End = Tok("end");

		internal static readonly Parser<char, string> CollectionName =
			Tok(Uppercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("collection name");

		internal static readonly Parser<char, string> InitLine =
			Tok(Init
			   .Then(CollectionName)
			   .Before(Colon)
			   );

		internal static readonly Parser<char, InitCollectionInstruction> InitInstruction =
			Tok(Map((collectionName, makeInstructions, optimizeInstructions)
						=> new InitCollectionInstruction(collectionName, makeInstructions, optimizeInstructions)
				  , InitLine
				  , MakeInstruction.Many()
				  , OptimizeInstruction.Many())
				   .Before(End)
			   );
	}

}
