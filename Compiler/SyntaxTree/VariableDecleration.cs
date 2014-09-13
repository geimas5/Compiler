namespace Compiler.SyntaxTree
{
    public abstract class VariableDecleration : StatementNode
    {
        public VariableNode Variable { get; set; }

        public VariableDecleration(Location location, VariableNode variable)
            : base(location)
        {
            this.Variable = variable;
        }
    }
}
