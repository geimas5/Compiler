namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class VariableArgument : Argument
    {
        public VariableArgument(VariableSymbol variable)
        {
            this.Variable = variable;
        }

        public VariableSymbol Variable { get; private set; }

        public override string ToString()
        {
            return Variable.Name;
        }
    }
}
