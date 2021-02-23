using System;
using System.Globalization;

namespace KoalaScript.Lang
{
	public abstract class KoalaLiteral<T> : KoalaType
	{
		public T RawValue { get; }

		public KoalaLiteral(T rawValue)
		{
			RawValue = rawValue;
		}

		public override int CountChildren() => 0;
		public override void GetChildren(Span<KoalaType> childrenReceiver) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => this;
		public override string ToString() => RawValue.ToString();
	}

	public class KString : KoalaLiteral<string>
	{
		public KString(string rawValue) : base(rawValue) { }
		public static implicit operator string(KString val) => val.RawValue;
		public static implicit operator KString(string val) => new KString(val);
		public override string ToString() => $"\"{RawValue}\"";
	}

	public class KNumber : KoalaLiteral<double>
	{
		public KNumber(double rawValue) : base(rawValue) { }
		public static implicit operator double(KNumber val) => val.RawValue;
		public static implicit operator KNumber(double val) => new KNumber(val);
		public static implicit operator KNumber(long val) => new KNumber(val);
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
		public VariableScope Scope { get; }

		public KVar(VariableScope scope, string rawValue)
			: base(rawValue)
		{
			Scope = scope;
		}

		public override string ToString() => $"{Scope.GetSymbol()}{RawValue}";
	}

}
