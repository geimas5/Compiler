namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public interface IReturningStatement
    {
        VariableSymbol Return { get; }
    }
}
