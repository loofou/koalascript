using System.Collections.Generic;
using System.Collections.Immutable;
using Pidgin;
using static Pidgin.Comment.CommentParser;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace PidginTest.KoalaScript
{

	public static class KoalaScriptParser
	{
		//Grammar Definition
		static readonly Parser<char, string> Comment = Try(String("//").Or(String("#")));
		static readonly Parser<char, Unit> SkipComments = SkipLineComment(Comment);

		static Parser<char, T> Tok<T>(Parser<char, T> p) => Try(p).Before(SkipWhitespaces);
		static Parser<char, string> Tok(string value) => Tok(String(value));
		static Parser<char, string> TokOr(string value1, string value2) => Tok(OneOf(String(value1), String(value2)));
		static Parser<char, char> Tok(char value) => Tok(Char(value));
		static Parser<char, char> TokOr(char value1, char value2) => Tok(OneOf(value1, value2));

		//Object creation
		static readonly Parser<char, string> Define = TokOr("define", "§");
		static readonly Parser<char, string> As = TokOr("as", ":");

		static readonly Parser<char, string> ObjectName =
			Tok(Uppercase.SelectMany(_ => OneOf(Letter, Digit, Char('_')).ManyString()
								   , (first, rest) => first + rest)).Labelled("object name");

		//Variable creation
		static readonly Parser<char, string> Set = TokOr("set", "%");
		static readonly Parser<char, string> To = TokOr("to", "=");

		static readonly Parser<char, string> VariableName =
			Tok(Lowercase.SelectMany(_ => OneOf(Letter, Digit, Char('_'), Char('-')).ManyString()
								   , (first, rest) => first + rest)).Labelled("variable name");

		//String Type
		static readonly Parser<char, char> Quote = Char('"');

		static readonly Parser<char, string> String =
			Tok(Token(c => c != '"')
			   .ManyString()
			   .Between(Quote)).Labelled("quoted string");

		//Number Type
		static readonly Parser<char, double> Number = Tok(Real).Labelled("number");

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

		static readonly Parser<char, bool> Bool = Try(True.Or(False)).Select(bool.Parse).Labelled("boolean");

		//Operation Type
		static readonly Parser<char, char> Add = Tok('+');
		static readonly Parser<char, char> Sub = Tok('-');
		static readonly Parser<char, char> Mul = Tok('*');
		static readonly Parser<char, char> Div = Tok('/');

		//Parsing
		static readonly Parser<char, KoalaObject> ObjectDefinition =
			Define
			   .Then(ObjectName)
			   .Before(As)
			   .SelectMany(_ => TypeDefinition.Separated(SkipWhitespaces),
						   (name, pairs) => new KoalaObject(name, pairs.ToImmutableDictionary()));

		static readonly Parser<char, KoalaType> StringType =
			String.Select(s => (KoalaType)new KString(s));

		static readonly Parser<char, KoalaType> NumberType =
			Number.Select(d => (KoalaType)new KNumber(d));

		static readonly Parser<char, KoalaType> BoolType =
			Bool.Select(b => (KoalaType)new KBool(b));

		static readonly Parser<char, KoalaType> VarRefType =
			VariableName.Select(s => (KoalaType)new KVarRef(s));

		static readonly Parser<char, KoalaType> LiteralType = Try(StringType.Or(NumberType).Or(BoolType).Or(VarRefType));

		static readonly Parser<char, KoalaType> AddOp = Map((type1, _, type2) => (KoalaType)new Add(type1, type2)
														  , LiteralType, Add, LiteralType);

		static readonly Parser<char, KoalaType> SubOp = Map((type1, _, type2) => (KoalaType)new Sub(type1, type2)
														  , LiteralType, Sub, LiteralType);

		static readonly Parser<char, KoalaType> Operation = Try(AddOp.Or(SubOp));

		static readonly Parser<char, KoalaType> Type = Try(Operation.Or(LiteralType));

		static readonly Parser<char, KeyValuePair<string, KoalaType>> TypeDefinition =
			Set
			   .Then(VariableName)
			   .Before(To)
			   .SelectMany(_ => Type, (name, type) => new KeyValuePair<string, KoalaType>(name, type)).Before(SkipWhitespaces);

		public static IEnumerable<KoalaObject> Parse(string input) => ObjectDefinition.Separated(SkipWhitespaces).ParseOrThrow(input);
	}

}
