namespace Compiler.Optimization
{
    using System.Collections.Generic;

    using Compiler.ControlFlowGraph;

    public class Optimizer
    {
        public Optimizer()
        {
            this.ActivatedOptimizations = new List<Optimizations>();
        }

        public IList<Optimizations> ActivatedOptimizations { get; private set; }

        public void RunOptimizations(ControlFlowGraph graph)
        {
            bool somethingChanged = true;

            while (somethingChanged)
            {
                somethingChanged = this.RunOptimizationPass(graph);
            }
        }

        private bool RunOptimizationPass(ControlFlowGraph graph)
        {
            bool somethingChanged = false;

            if (this.ActivatedOptimizations.Contains(Optimizations.EliminateEqualAssignments))
            {
                somethingChanged = new EqualAssignmentEliminator().RunOptimization(graph) || somethingChanged;
            }

            if (this.ActivatedOptimizations.Contains(Optimizations.LocalCopyPropagation))
            {
                somethingChanged = new LocalCopyPropagation().RunOptimization(graph) || somethingChanged;
            }

            if (this.ActivatedOptimizations.Contains(Optimizations.DeadCodeElimination))
            {
                somethingChanged = new DeadcodeEliminator().RunOptimization(graph) || somethingChanged;
            }

            return somethingChanged;
        }
    }
}
