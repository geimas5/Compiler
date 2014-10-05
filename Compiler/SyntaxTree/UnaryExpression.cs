namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class UnaryExpression : ExpressionNode
    {
        public UnaryExpression(Location location, UnaryOperator op, ExpressionNode expression)
            : base(location)
        {
            Trace.Assert(expression != null);

            this.Operator = op;
            this.Expression = expression;
        }

        public UnaryOperator Operator { get; private set; }

        public ExpressionNode Expression { get; private set; }

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
                yield return this.Expression;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Operator: {0})", this.Operator);
        }
    }
}
