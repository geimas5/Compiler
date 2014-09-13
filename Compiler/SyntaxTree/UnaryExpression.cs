namespace Compiler.SyntaxTree
{
    public class UnaryExpression : ExpressionNode
    {
        public UnaryExpression(Location location, UnaryOperator op, ExpressionNode expression)
            : base(location)
        {
            this.Operator = op;
            this.Expression = expression;
        }

        public UnaryOperator Operator { get; set; }

        public ExpressionNode Expression { get; set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Operator: {0})", this.Operator);
        }
    }
}
