using System;

namespace Joker.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator Op, BoundExpression operand)
        {
            Op = operand;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
    }
}