namespace Compiler.SyntaxTree
{
    public class UnInitializedVariableDecleration : VariableDecleration
    {
        public UnInitializedVariableDecleration(Location location, VariableNode variable)
            : base(location, variable)
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
