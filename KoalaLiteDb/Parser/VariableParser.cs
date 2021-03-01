using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;
using static Pidgin.Parser;
using static KoalaLiteDb.Parser.ParserHelper;
using static KoalaLiteDb.Parser.TypeParser;

namespace KoalaLiteDb.Parser
{

	internal static class VariableParser
	{
		static readonly Parser<char, char> Slash = Tok('/');
		static readonly Parser<char, string> Asterisk = Tok("*");
		static readonly Parser<char, string> Set = Tok("set");
		static readonly Parser<char, string> To = Tok("to");

		internal static readonly Parser<char, string> VariableName =
			Try(Tok(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
									   , (first, rest) => first + rest))).Labelled("variable name");

		internal static readonly Parser<char, IEnumerable<string>> VariablePath = Try(VariableName.Or(Asterisk).SeparatedAtLeastOnce(Slash));

		internal static readonly Parser<char, SetVarInstruction> SetVarInstruction =
			Line(Map((path, value) => new SetVarInstruction(path, value)
				   , Set.Then(VariablePath)
				   , To.Then(ValueType)
					)
					.Before(Dot));
	}

}
