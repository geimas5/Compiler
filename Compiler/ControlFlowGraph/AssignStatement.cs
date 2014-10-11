namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class AssignStatement : Statement, IReturningStatement
    {
        public AssignStatement(VariableSymbol destination, Argument argument)
        {
            this.Argument = argument;
            this.Return = destination;
        }

        public Argument Argument { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Return, Argument);
        }

        public VariableSymbol Return { get; private set; }
    }
}
