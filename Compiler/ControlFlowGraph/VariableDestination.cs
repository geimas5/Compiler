namespace Compiler.ControlFlowGraph
{
    using System.Diagnostics;

    using Compiler.SymbolTable;

    public class VariableDestination : Destination
    {
        public VariableDestination(VariableSymbol variable)
        {
            Trace.Assert(variable != null);

            this.Variable = variable;
        }

        public VariableSymbol Variable { get; private set; }

        public override Type Type
        {
            get
            {
                return Variable.Type;
            }
        }

        public override string ToString()
        {
            return this.Variable.ToString();
        }
    }
}
