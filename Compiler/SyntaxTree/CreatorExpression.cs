namespace Compiler.SyntaxTree
{
    public abstract class CreatorExpression : ExpressionNode
    {
        public CreatorExpression(Location location, TypeNode type)
            : base(location)
        {
            this.Type = type;
        }

        public TypeNode Type { get; set; }
    }
}
