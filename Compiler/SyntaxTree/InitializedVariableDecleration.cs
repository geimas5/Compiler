namespace Compiler.SyntaxTree
{
    public class InitializedVariableDecleration : VariableDecleration
    {
        public InitializedVariableDecleration(Location location, VariableNode variable, ExpressionNode initialization)
            : base(location, variable)
        {
            this.Initialization = initialization;
        }

        public ExpressionNode Initialization { get; set; }
    }
}
