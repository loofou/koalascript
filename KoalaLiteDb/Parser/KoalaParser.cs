using System.Collections.Generic;
using KoalaLiteDb.Lang;
using Pidgin;

namespace KoalaLiteDb.Parser
{

	public static class KoalaParser
	{
		public static readonly Parser<char, IEnumerable<InitCollectionInstruction>> MainParser = CollectionParser.InitInstruction.Many();
	}

}
