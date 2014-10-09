namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class AssignStatement : ReturningStatement
    {
        public AssignStatement(VariableSymbol destination, Argument argument)
            : base(destination)
        {
            this.Argument = argument;
        }

        public Argument Argument { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Return, Argument);
        }
    }
}
