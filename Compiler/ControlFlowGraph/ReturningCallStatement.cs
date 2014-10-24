namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class ReturningCallStatement : CallStatement, IReturningStatement
    {
        public ReturningCallStatement(FunctionSymbol function, int numberOfArguments, Destination destination)
            : base(function, numberOfArguments)
        {
            this.Return = destination;
        }

        public override string ToString()
        {
            return string.Format("{0} = Call {1}, {2}", this.Return, this.Function.Name, this.NumberOfArguments);
        }

        public Destination Return { get; private set; }
    }
}
