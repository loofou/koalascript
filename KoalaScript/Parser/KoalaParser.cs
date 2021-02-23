using System.Collections.Generic;
using KoalaScript.Lang;
using Pidgin;
using static Pidgin.Comment.CommentParser;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace KoalaScript.Parser
{

	public static class KoalaParser
	{
		//Grammar Definition
		public static readonly Parser<char, string> Comment = Try(String("//").Or(String("#")));
		public static readonly Parser<char, Unit> SkipComments = SkipLineComment(Comment);

		public static Parser<char, T> Tok<T>(Parser<char, T> p) => Try(p).Before(SkipWhitespaces);
		public static Parser<char, string> Tok(string value) => Tok(String(value));
		public static Parser<char, char> Tok(char value) => Tok(Char(value));

		//Variable creation
		public static readonly Parser<char, string> Set = Tok("set");
		public static readonly Parser<char, string> To = Tok("to");

		public static readonly Parser<char, char> Percent = Char('%');
		public static readonly Parser<char, char> Dollar = Char('$');
		public static readonly Parser<char, char> Tilde = Char('~');

		public static readonly Parser<char, VariableScope> VariableScope =
			Try(OneOf(Percent, Dollar, Tilde))
			   .Select(KoalaEnumExtensions.GetVariableScopeFromChar).Labelled("variable scope symbol ($, % or ~)");

		public static readonly Parser<char, string> VariableName =
			Tok(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("variable name");

		//String Type
		public static readonly Parser<char, char> Quote = Char('"');

		public static readonly Parser<char, string> String =
			Tok(Token(c => c != '"')
			   .ManyString()
			   .Between(Quote)).Labelled("quoted string");

		//Number Type
		public static readonly Parser<char, double> Number = Tok(Real).Labelled("number");

		//Bool Type
		public static readonly Parser<char, string> True
			= Tok(OneOf(
						String("true")
					  , String("True")
					  , String("yes")
					  , String("Yes")
					  , String("on")
					  , String("On")
					   ));

		public static readonly Parser<char, string> False =
			Tok(OneOf(
					  String("false")
					, String("False")
					, String("no")
					, String("No")
					, String("off")
					, String("Off")
					 ));

		public static readonly Parser<char, bool> Bool = Try(True.Or(False)).Select(bool.Parse).Labelled("boolean");

		//Operation Type
		public static readonly Parser<char, char> Add = Tok('+');
		public static readonly Parser<char, char> Sub = Tok('-');
		public static readonly Parser<char, char> Mul = Tok('*');
		public static readonly Parser<char, char> Div = Tok('/');

		//Parsing
		/*static readonly Parser<char, KoalaMap> ObjectDefinition =
			Define
			   .Then(ObjectName)
			   .Before(As)
			   .SelectMany(_ => TypeDefinition.Separated(SkipWhitespaces),
						   (name, pairs) => new KoalaMap(pairs.ToDictionary()));*/

		public static readonly Parser<char, KoalaType> StringType =
			String.Select(s => (KoalaType)new KString(s));

		public static readonly Parser<char, KoalaType> NumberType =
			Number.Select(d => (KoalaType)new KNumber(d));

		public static readonly Parser<char, KoalaType> BoolType =
			Bool.Select(b => (KoalaType)new KBool(b));

		public static readonly Parser<char, KoalaType> VarType =
			Map((symbol, name) => (KoalaType)new KVar(symbol, name)
			  , VariableScope, VariableName);

		public static readonly Parser<char, KoalaType> LiteralType = Try(StringType.Or(NumberType).Or(BoolType).Or(VarType));

		public static readonly Parser<char, KoalaType> AddOp =
			Map((type1, _, type2) => (KoalaType)new Add(type1, type2)
			  , LiteralType, Add, LiteralType);

		public static readonly Parser<char, KoalaType> SubOp =
			Map((type1, _, type2) => (KoalaType)new Sub(type1, type2)
			  , LiteralType, Sub, LiteralType);

		public static readonly Parser<char, KoalaType> Operation = Try(AddOp.Or(SubOp));

		public static readonly Parser<char, KoalaType> Type = Try(Operation.Or(LiteralType));

		public static readonly Parser<char, KeyValuePair<KVar, KoalaType>> TypeDefinition =
			Set
			   .Then(VarType)
			   .Before(To)
			   .SelectMany(_ => Type, (name, type) => new KeyValuePair<KVar, KoalaType>((KVar)name, type)).Before(SkipWhitespaces);

		//public static IEnumerable<KoalaMap> Parse(string input) => ObjectDefinition.Separated(SkipWhitespaces).ParseOrThrow(input);
	}

}
