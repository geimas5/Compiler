namespace Compiler.SyntaxTree
{
    public abstract class ExpressionNode : Node
    {
        protected ExpressionNode(Location location)
            : base(location)
        {
        }

        public Type ResultingType { get; set; }
    }
}
