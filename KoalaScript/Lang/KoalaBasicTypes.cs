using System;
using System.Globalization;

namespace KoalaScript.Lang
{

	public abstract class KoalaLiteral<T> : KoalaType, IEquatable<T>, IComparable<T>
		where T : IComparable<T>, IEquatable<T>
	{
		public T RawValue { get; }

		protected KoalaLiteral(T rawValue)
		{
			RawValue = rawValue ?? throw new ArgumentNullException(nameof(rawValue));
		}

		public override object GetValue() => RawValue;
		public virtual T GetValueCasted() => RawValue;

		public override int CountChildren() => 0;
		public override void GetChildren(Span<KoalaType> childrenReceiver) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => this;

		public bool Equals(T other) => RawValue.Equals(other);
		public int CompareTo(T other) => RawValue.CompareTo(other);

		public override string ToString() => GetValue().ToString();
	}

	public class KString : KoalaLiteral<string>
	{
		public KString(string rawValue) : base(rawValue) { }
		public static implicit operator string(KString val) => val.RawValue;
		public static implicit operator KString(string val) => new KString(val);
		public override string ToString() => $"\"{GetValue()}\"";
	}

	public class KNumber : KoalaLiteral<int>
	{
		public KNumber(int rawValue) : base(rawValue) { }
		public static implicit operator double(KNumber val) => val.RawValue;
		public static implicit operator KNumber(int val) => new KNumber(val);
		public override string ToString() => RawValue.ToString(NumberFormatInfo.InvariantInfo);
	}

	public class KBool : KoalaLiteral<bool>
	{
		public KBool(bool rawValue) : base(rawValue) { }
		public static implicit operator bool(KBool val) => val.RawValue;
		public static implicit operator KBool(bool val) => new KBool(val);
	}

	public class KVar : KoalaLiteral<string>
	{
		public KVar(string rawValue) : base(rawValue) { }
	}

}
