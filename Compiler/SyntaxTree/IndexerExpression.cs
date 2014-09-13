namespace Compiler.SyntaxTree
{
    public class IndexerExpression : ExpressionNode
    {
        public IndexerExpression(Location location, ExpressionNode name, ExpressionNode index)
            : base(location)
        {
            this.Name = name;
            this.Index = index;
        }

        public ExpressionNode Name { get; set; }

        public ExpressionNode Index { get; set; }

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
