using System.Collections.Immutable;
using KoalaScript.Lang;
using Pidgin;
using static Pidgin.Parser;
using static KoalaScript.Parser.GenericParser;

namespace KoalaScript.Parser
{

	public static class KoalaConfigParser
	{
		internal static readonly Parser<char, ImmutableDictionary<KVar, KoalaType>> ConfigVariables =
			Try(TypeDefinition.Many().Select(list => list.ToImmutableDictionary())).Labelled("config variables");
	}

}
