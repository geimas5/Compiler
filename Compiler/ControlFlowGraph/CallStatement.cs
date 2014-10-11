namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class CallStatement : Statement
    {
        public CallStatement(FunctionSymbol function, int numberOfArguments)
        {
            this.Function = function;
            this.NumberOfArguments = numberOfArguments;
        }

        public FunctionSymbol Function { get; set; }

        public int NumberOfArguments { get; set; }

        public override string ToString()
        {
            return "Call " + this.Function.Name + ", " + NumberOfArguments;
        }
    }
}
