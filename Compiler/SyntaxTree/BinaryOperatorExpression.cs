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
    }
}
