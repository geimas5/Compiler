namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class BinaryOperatorExpression : ExpressionNode
    {
        public BinaryOperatorExpression(Location location, ExpressionNode left, ExpressionNode right, BinaryOperator op)
            : base(location)
        {
            Trace.Assert(left != null);
            Trace.Assert(right != null);

            this.Left = left;
            this.Right = right;
            this.Operator = op;
        }

        public ExpressionNode Left { get; private set; }

        public ExpressionNode Right { get; private set; }

        public BinaryOperator Operator { get; private set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IEnumerable<Node> Children
        {
            get
            {
                return new[] { this.Left, this.Right };
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Operator: {0})", this.Operator);
        }
    }
}
