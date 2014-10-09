namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public abstract class ReturningStatement : Statement
    {
        protected ReturningStatement(VariableSymbol @return)
        {
            this.Return = @return;
        }

        public VariableSymbol Return { get; set; }
    }
}
