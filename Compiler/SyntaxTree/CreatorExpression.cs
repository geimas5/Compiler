namespace Compiler.SyntaxTree
{
    public abstract class CreatorExpression : ExpressionNode
    {
        public CreatorExpression(Location location, PrimitiveType type)
            : base(location)
        {
            this.Type = type;
        }

        public PrimitiveType Type { get; set; }

    }
}
