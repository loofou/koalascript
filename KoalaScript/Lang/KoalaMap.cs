using System;
using System.Collections.Generic;
using System.Linq;

namespace KoalaScript.Lang
{

	public class KoalaMap : KoalaType
	{
		public Dictionary<KVar, KoalaType> Value { get; }

		public KoalaMap(Dictionary<KVar, KoalaType> value)
		{
			Value = value;
		}

		public override object GetValue() => Value;

		public override int CountChildren() => 0;
		public override void GetChildren(Span<KoalaType> childrenReceiver) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => this;

		public override string ToString() => $"( {string.Join(", ", Value.Select(pair => $"{pair.Key}: {pair.Value}"))} )";
	}

}
