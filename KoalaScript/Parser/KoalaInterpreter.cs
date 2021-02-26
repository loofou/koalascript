using System.Collections.Generic;
using System.Collections.Immutable;
using KoalaScript.Lang;
using Pidgin;
using ServiceStack;

namespace KoalaScript.Parser
{

	public static class KoalaInterpreter
	{
		public static IEnumerable<KeyValuePair<KVar, KoalaType>> Run(string scriptFile)
		{
			return null;
			//return GenericParser.RootVariables.ParseOrThrow(scriptFile);
		}

		public static T LoadConfig<T>(string script, in T config) where T : IKoalaConfig
		{
			ImmutableDictionary<KVar, KoalaType> configVars = KoalaConfigParser.ConfigVariables.ParseOrThrow(script);

			Dictionary<string, object> configMap = config.ToObjectDictionary();

			foreach(KeyValuePair<KVar, KoalaType> pair in configVars)
			{
				string key = pair.Key.RawValue;
				if(configMap.ContainsKey(key))
				{
					configMap[key] = pair.Value.GetValue();
				}
				else
				{
					configMap.Add(key, pair.Value.GetValue());
				}
			}

			return configMap.FromObjectDictionary<T>();
		}
	}

}
