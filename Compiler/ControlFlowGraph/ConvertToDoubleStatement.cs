namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class ConvertToDoubleStatement : Statement, IReturningStatement
    {
        public ConvertToDoubleStatement(VariableSymbol @return, Argument argument)
        {
            this.Return = @return;
            this.Argument = argument;
        }

        public VariableSymbol Return { get; private set; }

        public Argument Argument { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = ConvertToDouble({1})", this.Return, this.Argument);
        }
    }
}
