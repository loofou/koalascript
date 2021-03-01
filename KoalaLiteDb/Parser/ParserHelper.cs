using Pidgin;
using static Pidgin.Comment.CommentParser;
using static Pidgin.Parser;

namespace KoalaLiteDb.Parser
{

	internal static class ParserHelper
	{
		static readonly Parser<char, string> Hash = String("#");

		static readonly Parser<char, Unit> SkipComments = SkipLineComment(Hash).Before(SkipWhitespaces);

		internal static Parser<char, T> Tok<T>(Parser<char, T> p) =>
			Try(SkipComments.Optional()
							.Then(p
								 .Before(SkipWhitespaces)
								 .Before(SkipComments.Optional())));

		internal static Parser<char, string> Tok(string value) => Tok(String(value));
		internal static Parser<char, char> Tok(char value) => Tok(Char(value));

		//internal static Parser<char, T> Line<T>(Parser<char, T> p) => Try(SkipComments.Optional().Then(p.Before(SkipComments.Optional())));
	}

}
