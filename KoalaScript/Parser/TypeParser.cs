using KoalaScript.Lang;
using Pidgin;
using ServiceStack;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using static KoalaScript.Parser.GenericParser;

namespace KoalaScript.Parser
{

	internal static class TypeParser
	{
		internal static readonly Parser<char, string> VariableName =
			Keyword(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
									   , (first, rest) => first + rest)).Labelled("variable name");

		//String Type
		internal static readonly Parser<char, char> Quote = Char('"');

		internal static readonly Parser<char, string> String =
			Keyword(Token(c => c != '"')
				   .ManyString()
				   .Between(Quote)).Labelled("quoted string");

		//Number Type
		internal static readonly Parser<char, int> Number = Keyword(Num).Labelled("number");

		//Bool Type
		internal static readonly Parser<char, string> True
			= Keyword(OneOf(
							String("true")
						  , String("True")
						  , String("yes")
						  , String("Yes")
						  , String("on")
						  , String("On")
						   ));

		internal static readonly Parser<char, string> False =
			Keyword(OneOf(
						  String("false")
						, String("False")
						, String("no")
						, String("No")
						, String("off")
						, String("Off")
						 ));

		internal static readonly Parser<char, bool> Bool =
			Keyword(True.Or(False))
			   .Select(boolean =>
				{
					if(boolean.EqualsIgnoreCase("true")
					|| boolean.EqualsIgnoreCase("on")
					|| boolean.EqualsIgnoreCase("yes"))
					{
						return true;
					}

					if(boolean.EqualsIgnoreCase("false")
					|| boolean.EqualsIgnoreCase("off")
					|| boolean.EqualsIgnoreCase("no"))
					{
						return false;
					}

					return bool.Parse(boolean);
				})
			   .Labelled("boolean");

		//Parsers
		internal static readonly Parser<char, KoalaType> StringType =
			String.Select(s => (KoalaType)new KString(s)).Labelled("string type");

		internal static readonly Parser<char, KoalaType> NumberType =
			Number.Select(d => (KoalaType)new KNumber(d)).Labelled("number type");

		internal static readonly Parser<char, KoalaType> BoolType =
			Bool.Select(b => (KoalaType)new KBool(b)).Labelled("bool type");

		internal static readonly Parser<char, KoalaType> VarType =
			VariableName.Select(name => (KoalaType)new KVar(name)).Labelled("variable type");

		internal static readonly Parser<char, KoalaType> LiteralType = Try(StringType.Or(NumberType).Or(BoolType).Or(VarType));
	}

}
