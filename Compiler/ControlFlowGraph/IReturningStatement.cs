namespace Compiler.ControlFlowGraph
{
    public interface IReturningStatement
    {
        Destination Return { get; }
    }
}
