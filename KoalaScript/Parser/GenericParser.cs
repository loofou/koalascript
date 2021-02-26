using System.Collections.Generic;
using KoalaScript.Lang;
using Pidgin;
using static KoalaScript.Parser.TypeParser;
using static Pidgin.Comment.CommentParser;
using static Pidgin.Parser;

namespace KoalaScript.Parser
{

	internal static class GenericParser
	{
		//Grammar Definition
		internal static readonly Parser<char, string> Hash = String("#");
		internal static readonly Parser<char, Unit> SkipComments = SkipLineComment(Hash).Before(SkipWhitespaces);

		internal static Parser<char, T> Keyword<T>(Parser<char, T> p) => Try(p.Before(SkipWhitespaces));
		internal static Parser<char, string> Keyword(string value) => Keyword(String(value));
		internal static Parser<char, char> Keyword(char value) => Keyword(Char(value));

		internal static Parser<char, T> Line<T>(Parser<char, T> p) =>
			Try(SkipComments.Optional()
							.Then(p
								 .Before(Dot)
								 .Before(SkipComments.Optional())));

		//Variable creation
		internal static readonly Parser<char, string> Set = Keyword("set");
		internal static readonly Parser<char, string> To = Keyword("to");
		internal static readonly Parser<char, char> Dot = Keyword('.');

		//Parsing

		/*internal static readonly Parser<char, KoalaType> AddOp =
			Map((type1, _, type2) => (KoalaType)new Add(type1, type2)
			  , LiteralType, Add, LiteralType);

		internal static readonly Parser<char, KoalaType> SubOp =
			Map((type1, _, type2) => (KoalaType)new Sub(type1, type2)
			  , LiteralType, Sub, LiteralType);

		internal static readonly Parser<char, KoalaType> Operation = Try(AddOp.Or(SubOp)).Labelled("operation");*/

		internal static readonly Parser<char, KoalaType> Type = Try(LiteralType).Labelled("type");

		internal static readonly Parser<char, KeyValuePair<KVar, KoalaType>> TypeDefinition =
			Line(Set
				.Then(VarType)
				.Before(To)
				.SelectMany(_ => Type, (name, type) => new KeyValuePair<KVar, KoalaType>((KVar)name, type))
				)
			   .Labelled("type definition (set x to y.)");
	}

}
