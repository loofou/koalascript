using System;

namespace KoalaScript.Lang
{

	public enum VariableScope
	{
		Temp
	  , Local
	  , Global
	}

	public static class KoalaEnumExtensions
	{
		public static string GetSymbol(this VariableScope scope) =>
			scope switch
				{
					VariableScope.Temp => "~"
				  , VariableScope.Local => "$"
				  , VariableScope.Global => "%"
				  , _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
				};

		public static VariableScope GetVariableScopeFromChar(char symbol) =>
			symbol switch
				{
					'~' => VariableScope.Temp
				  , '$' => VariableScope.Local
				  , '%' => VariableScope.Global
				  , _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null)
				};
	}

}
