using System;
using Pidgin;
using UltraLiteDB;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using static KoalaLiteDb.Parser.ParserHelper;

namespace KoalaLiteDb.Parser
{

	internal static class TypeParser
	{
		//String Type
		static readonly Parser<char, char> Quote = Char('"');

		internal static readonly Parser<char, string> String =
			Tok(Token(c => c != '"')
			   .ManyString()
			   .Between(Quote)).Labelled("quoted string");

		//Number Type
		internal static readonly Parser<char, int> Number = Tok(Num).Labelled("number");

		//Bool Type
		static readonly Parser<char, string> True
			= Tok(OneOf(
						String("true")
					  , String("True")
					  , String("yes")
					  , String("Yes")
					  , String("on")
					  , String("On")
					   ));

		static readonly Parser<char, string> False =
			Tok(OneOf(
					  String("false")
					, String("False")
					, String("no")
					, String("No")
					, String("off")
					, String("Off")
					 ));

		internal static readonly Parser<char, bool> Bool =
			Tok(True.Or(False))
			   .Select(boolean => boolean.Equals("true", StringComparison.InvariantCultureIgnoreCase)
							   || boolean.Equals("on", StringComparison.InvariantCultureIgnoreCase)
							   || boolean.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
			   .Labelled("boolean");

		//Parsers
		internal static readonly Parser<char, BsonValue> StringType =
			String.Select(s => new BsonValue(s)).Labelled("string type");

		internal static readonly Parser<char, BsonValue> NumberType =
			Number.Select(d => new BsonValue(d)).Labelled("number type");

		internal static readonly Parser<char, BsonValue> BoolType =
			Bool.Select(b => new BsonValue(b)).Labelled("bool type");

		internal static readonly Parser<char, BsonValue> ValueType = Try(StringType.Or(NumberType).Or(BoolType));
	}

}
