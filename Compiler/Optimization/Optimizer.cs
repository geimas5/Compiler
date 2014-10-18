namespace Compiler.Optimization
{
    using System.Collections.Generic;

    public class Optimizer
    {
        public Optimizer()
        {
            this.ActivatedOptimizations = new List<Optimizations>();
        }

        public IList<Optimizations> ActivatedOptimizations { get; private set; }

        public void RunOptimizations(ControlFlowGraph.ControlFlowGraph graph)
        {
            if (this.ActivatedOptimizations.Contains(Optimizations.EliminateEqualAssignments))
            {
                new EqualAssignmentEliminator().RunOptimization(graph);
            }
        }
    }
}
