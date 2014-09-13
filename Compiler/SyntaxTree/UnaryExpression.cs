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
    }
}
