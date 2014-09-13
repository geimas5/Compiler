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
    }
}
