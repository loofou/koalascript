using System.Collections.Generic;
using KoalaScript.Lang;
using Pidgin;
using ServiceStack;
using static Pidgin.Comment.CommentParser;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace KoalaScript.Parser
{

	public static class KoalaParser
	{
		//Grammar Definition
		static readonly Parser<char, string> Comment = String("#");
		static readonly Parser<char, Unit> SkipComments = Whitespaces.Then(SkipLineComment(Comment));
		static readonly Parser<char, Unit> SkipToEnd = Whitespaces.Then(End.Or(Try(EndOfLine).IgnoreResult()));

		static readonly Parser<char, Unit> SkipEmptyOrCommentLines = Try(Whitespaces.Then(Try(SkipComments.Or(SkipToEnd))).SkipMany());

		static Parser<char, T> Keyword<T>(Parser<char, T> p) => Try(p.Before(Whitespace));
		static Parser<char, string> Keyword(string value) => Keyword(String(value));
		static Parser<char, char> Keyword(char value) => Keyword(Char(value));
		static Parser<char, T> Line<T>(Parser<char, T> p) => Try(p.Before(Try(SkipComments.Or(SkipToEnd))));

		//Variable creation
		static readonly Parser<char, string> Set = Keyword("set");
		static readonly Parser<char, string> To = Keyword("to");

		static readonly Parser<char, char> Percent = Char('%');
		static readonly Parser<char, char> Dollar = Char('$');
		static readonly Parser<char, char> Tilde = Char('~');

		static readonly Parser<char, VariableScope> VariableScope =
			Try(OneOf(Percent, Dollar, Tilde))
			   .Select(KoalaEnumExtensions.GetVariableScopeFromChar).Labelled("variable scope symbol ($, % or ~)");

		static readonly Parser<char, string> VariableName =
			Try(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("variable name");

		//String Type
		static readonly Parser<char, char> Quote = Char('"');

		static readonly Parser<char, string> String =
			Try(Token(c => c != '"')
			   .ManyString()
			   .Between(Quote)).Labelled("quoted string");

		//Number Type
		static readonly Parser<char, double> Number = Try(Real).Labelled("number");

		//Bool Type
		static readonly Parser<char, string> True
			= Try(OneOf(
						String("true")
					  , String("True")
					  , String("yes")
					  , String("Yes")
					  , String("on")
					  , String("On")
					   ));

		static readonly Parser<char, string> False =
			Try(OneOf(
					  String("false")
					, String("False")
					, String("no")
					, String("No")
					, String("off")
					, String("Off")
					 ));

		static readonly Parser<char, bool> Bool =
			Try(True.Or(False))
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

		//Operation Type
		static readonly Parser<char, char> Add = Char('+').Between(SkipWhitespaces);
		static readonly Parser<char, char> Sub = Char('-').Between(SkipWhitespaces);
		static readonly Parser<char, char> Mul = Char('*').Between(SkipWhitespaces);
		static readonly Parser<char, char> Div = Char('/').Between(SkipWhitespaces);

		//Parsing
		static readonly Parser<char, KoalaType> StringType =
			String.Select(s => (KoalaType)new KString(s)).Labelled("string type");

		static readonly Parser<char, KoalaType> NumberType =
			Number.Select(d => (KoalaType)new KNumber(d)).Labelled("number type");

		static readonly Parser<char, KoalaType> BoolType =
			Bool.Select(b => (KoalaType)new KBool(b)).Labelled("bool type");

		static readonly Parser<char, KoalaType> VarType =
			Try(Map((symbol, name) => (KoalaType)new KVar(symbol, name), VariableScope, VariableName)
				   .Or(VariableName.Select(name => (KoalaType)new KVar(Lang.VariableScope.Local, name)))).Labelled("variable type");

		static readonly Parser<char, KoalaType> LiteralType = Try(StringType.Or(NumberType).Or(BoolType).Or(VarType));

		static readonly Parser<char, KoalaType> AddOp =
			Map((type1, _, type2) => (KoalaType)new Add(type1, type2)
			  , LiteralType, Add, LiteralType);

		static readonly Parser<char, KoalaType> SubOp =
			Map((type1, _, type2) => (KoalaType)new Sub(type1, type2)
			  , LiteralType, Sub, LiteralType);

		static readonly Parser<char, KoalaType> Operation = Try(AddOp.Or(SubOp)).Labelled("operation");

		static readonly Parser<char, KoalaType> Type = Try(Operation.Or(LiteralType)).Labelled("type");

		public static readonly Parser<char, KeyValuePair<KVar, KoalaType>> TypeDefinition =
			Line(Set
				.Then(VarType)
				.Before(To)
				.SelectMany(_ => Type, (name, type) => new KeyValuePair<KVar, KoalaType>((KVar)name, type))).Labelled("type definition");

		public static readonly Parser<char, IEnumerable<KeyValuePair<KVar, KoalaType>>> RootVariables =
			Try(TypeDefinition.SeparatedAndOptionallyTerminated(EndOfLine).Select(list => list)).Labelled("root variable");
	}

}
