namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class PointerArgument : Argument
    {
        public PointerArgument(VariableSymbol variable, Type type)
            : base(type)
        {
            this.Variable = variable;
        }

        public VariableSymbol Variable { get; private set; }

        public override string ToString()
        {
            if (Variable.Register.HasValue)
            {
                return "*(" + Variable.Register + ")";    
            }

            return "*(" + Variable.Name + ")";
        }
    }
}
