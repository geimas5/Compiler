namespace Compiler.SyntaxTree
{
    public abstract class ReturnStatement : StatementNode
    {
        public ReturnStatement(Location location)
            : base(location)
        {
        }
    }
}
