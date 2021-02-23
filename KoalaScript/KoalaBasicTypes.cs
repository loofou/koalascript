using System;
using System.Globalization;
using Sawmill;

namespace PidginTest.KoalaScript
{

	public enum KoalaFormatting
	{
		Long
	  , Short
	  , Minimal
	}

	public interface IKoalaType
	{
		string ToString(KoalaFormatting formatting);
	}

	public abstract class KoalaType : IKoalaType, IRewritable<KoalaType>
	{
		public abstract int CountChildren();
		public abstract void GetChildren(Span<KoalaType> childrenReceiver);
		public abstract KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren);

		public virtual string ToString(KoalaFormatting formatting) => ToString();
	}

	public abstract class KoalaLiteral<T> : KoalaType
	{
		public T Value { get; }

		public KoalaLiteral(T value)
		{
			Value = value;
		}

		public override int CountChildren() => 0;
		public override void GetChildren(Span<KoalaType> childrenReceiver) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => this;
		public override string ToString() => Value.ToString();
	}

	public class String : KoalaLiteral<string>
	{
		public String(string value) : base(value) { }

		public override string ToString() => $"\"{Value}\"";
	}

	public class Number : KoalaLiteral<double>
	{
		public Number(double value) : base(value) { }

		public override string ToString() => Value.ToString(NumberFormatInfo.InvariantInfo);
	}

	public class Bool : KoalaLiteral<bool>
	{
		public Bool(bool value) : base(value) { }
	}

	public class Ref : KoalaLiteral<string>
	{
		public Ref(string value) : base(value) { }
	}

}
