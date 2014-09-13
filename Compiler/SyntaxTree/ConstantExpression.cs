namespace Compiler.SyntaxTree
{
    public class ConstantExpression : ExpressionNode
    {
        public ConstantExpression(Location location, ConstantNode constant)
            : base(location)
        {
            this.Constant = constant;
        }

        public ConstantNode Constant { get; set; }
    }
}
