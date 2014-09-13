namespace Compiler.SyntaxTree
{
    public abstract class ConstantNode : Node
    {
        protected ConstantNode(Location location)
            : base(location)
        {
        }
    }
}
