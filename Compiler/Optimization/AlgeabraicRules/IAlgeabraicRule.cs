namespace Compiler.Optimization.AlgeabraicRules
{
    using Compiler.ControlFlowGraph;

    public interface IAlgeabraicRule
    {
        bool ProcessStatement(Statement statement);
    }
}
