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

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
