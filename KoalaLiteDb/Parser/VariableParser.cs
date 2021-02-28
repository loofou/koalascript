using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;
using UltraLiteDB;
using static Pidgin.Parser;
using static KoalaLiteDb.Parser.ParserHelper;
using static KoalaLiteDb.Parser.TypeParser;

namespace KoalaLiteDb.Parser
{

	internal static class VariableParser
	{
		static readonly Parser<char, char> Slash = Tok('/');
		static readonly Parser<char, string> Set = Tok("set");
		static readonly Parser<char, string> To = Tok("to");

		internal static readonly Parser<char, string> VariableName =
			Tok(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("variable name");

		internal static readonly Parser<char, string> CollectionName =
			Tok(Uppercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("collection name");

		internal static readonly Parser<char, List<string>> VariablePath =
			Try(
				Map((first, rest) =>
					{
						List<string> list = new() { first };
						list.AddRange(rest);
						return list;
					}
				  , CollectionName.Before(Slash)
				  , VariableName.SeparatedAtLeastOnce(Slash))
			   );

		internal static readonly Parser<char, IKoalaInstruction> SetVarInstruction =
			Tok(
				Set
				   .Then(VariablePath)
				   .Before(To)
				   .SelectMany(_ => ValueType,
							   (path, value) => (IKoalaInstruction)new SetVarInstruction(path, value))
			   );
	}

}
