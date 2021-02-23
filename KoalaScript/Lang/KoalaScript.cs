using System;
using System.Collections.Generic;
using Sawmill;

namespace KoalaScript.Lang
{
	public abstract class KoalaType : IRewritable<KoalaType>
	{
		public abstract int CountChildren();
		public abstract void GetChildren(Span<KoalaType> childrenReceiver);
		public abstract KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren);
	}

	public class KoalaScript
	{
		public Dictionary<KVar, KoalaType> Globals;
	}
}
