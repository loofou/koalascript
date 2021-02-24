using System.Collections.Generic;
using KoalaScript.Lang;
using Pidgin;

namespace KoalaScript.Parser
{

	public class KoalaInterpreter
	{
		public static Result<char, IEnumerable<KeyValuePair<KVar, KoalaType>>> Run(string scriptFile)
		{
			return KoalaParser.RootVariables.Parse(scriptFile);
		}
	}

}
