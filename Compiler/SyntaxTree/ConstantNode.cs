namespace Compiler.SyntaxTree
{
    public abstract class ConstantNode : ExpressionNode
    {
        protected ConstantNode(Location location)
            : base(location)
        {
        }
    }
}
