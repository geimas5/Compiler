namespace Compiler.SyntaxTree
{
    public class VoidReturnStatement : ReturnStatement
    {
        public VoidReturnStatement(Location location)
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
