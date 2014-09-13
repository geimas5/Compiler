namespace Compiler.SyntaxTree
{
    public class ArrayCreatorExpression : CreatorExpression
    {
        public ArrayCreatorExpression(Location location, TypeNode type, ExpressionNode size)
            : base(location, type)
        {
            this.Size = size;
        }

        public ExpressionNode Size { get; set; }

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
