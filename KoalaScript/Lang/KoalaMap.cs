using System;
using System.Collections.Generic;

namespace KoalaScript.Lang
{

	public class KoalaMap : KoalaType
	{
		public Dictionary<KVar, KoalaType> Value { get; }

		public KoalaMap(Dictionary<KVar, KoalaType> value)
		{
			Value = value;
		}

		public override int CountChildren() => 0;
		public override void GetChildren(Span<KoalaType> childrenReceiver) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => this;

		public override string ToString()
		{
			//TODO
			return base.ToString();
		}
	}

}
