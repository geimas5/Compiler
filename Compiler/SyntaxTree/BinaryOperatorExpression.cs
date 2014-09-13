namespace Compiler.SyntaxTree
{
    public class BinaryOperatorExpression : ExpressionNode
    {
        public BinaryOperatorExpression(Location location, ExpressionNode left, ExpressionNode right, BinaryOperator op)
            : base(location)
        {
            this.Left = left;
            this.Right = right;
            this.Operator = op;
        }

        public ExpressionNode Left { get; private set; }

        public ExpressionNode Right { get; private set; }

        public BinaryOperator Operator { get; set; }

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
