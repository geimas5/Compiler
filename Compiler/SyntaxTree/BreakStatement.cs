namespace Compiler.SyntaxTree
{
    public class BreakStatement : StatementNode
    {
        public BreakStatement(Location location)
            : base(location)
        {
        }

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
