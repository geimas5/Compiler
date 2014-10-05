namespace Compiler.SyntaxTree
{
    using System.Diagnostics;

    public abstract class VariableDecleration : StatementNode
    {
        public VariableNode Variable { get; set; }

        public VariableDecleration(Location location, VariableNode variable)
            : base(location)
        {
            Trace.Assert(variable != null);
            this.Variable = variable;
        }
    }
}
