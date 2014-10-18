namespace Compiler.Optimization
{
    public abstract class OptimizerBase
    {
        public abstract bool RunOptimization(ControlFlowGraph.ControlFlowGraph graph);
    }
}
