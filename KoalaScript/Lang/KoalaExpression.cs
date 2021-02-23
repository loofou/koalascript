using System;

namespace KoalaScript.Lang
{

	public abstract class UnaryExpr : KoalaType
	{
		public KoalaType Operand { get; }

		public UnaryExpr(KoalaType operand)
		{
			Operand = operand;
		}

		public override int CountChildren() => 1;

		public override void GetChildren(Span<KoalaType> childrenReceiver)
		{
			childrenReceiver[0] = Operand;
		}

		protected abstract string GetOperatorSymbol();
		public override string ToString() => $"{GetOperatorSymbol()}{Operand}";
	}

	public abstract class BinaryExpr : KoalaType
	{
		public KoalaType Left { get; }
		public KoalaType Right { get; }

		public BinaryExpr(KoalaType left, KoalaType right)
		{
			Left = left;
			Right = right;
		}

		public override int CountChildren() => 2;

		public override void GetChildren(Span<KoalaType> childrenReceiver)
		{
			childrenReceiver[0] = Left;
			childrenReceiver[1] = Right;
		}

		protected abstract string GetOperatorSymbol();

		public override string ToString() => $"{Left} {GetOperatorSymbol()} {Right}";
	}

	public class Add : BinaryExpr
	{
		public Add(KoalaType left, KoalaType right) : base(left, right) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => new Add(newChildren[0], newChildren[1]);
		protected override string GetOperatorSymbol() => "+";
	}
	
	public class Sub : BinaryExpr
	{
		public Sub(KoalaType left, KoalaType right) : base(left, right) { }
		public override KoalaType SetChildren(ReadOnlySpan<KoalaType> newChildren) => new Sub(newChildren[0], newChildren[1]);
		protected override string GetOperatorSymbol() => "-";
	}

}
