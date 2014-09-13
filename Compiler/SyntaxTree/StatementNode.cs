namespace Compiler.SyntaxTree
{
    public abstract class StatementNode : Node
    {
        protected StatementNode(Location location)
            : base(location)
        {
        }
    }
}
