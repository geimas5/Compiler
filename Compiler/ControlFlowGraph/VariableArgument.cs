namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class VariableArgument : Argument
    {
        public VariableArgument(VariableSymbol variable)
            : base(variable.Type)
        {
            this.Variable = variable;
        }

        public VariableSymbol Variable { get; private set; }

        public override string ToString()
        {
            if (Variable.Register.HasValue)
            {
                return Variable.Register.Value.ToString();
            }

            return Variable.Name;
        }
    }
}
