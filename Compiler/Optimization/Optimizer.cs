namespace Compiler.Optimization
{
    using System;
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
            if (this.ActivatedOptimizations.Contains(Optimizations.LocalCopyPropagation)
                && !this.ActivatedOptimizations.Contains(Optimizations.EliminateEqualAssignments))
            {
                throw new Exception("Using local copy propagation requires also "
                                    + "using the eliminate equal assignments optimization");
            }

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

            if (this.ActivatedOptimizations.Contains(Optimizations.AlgebraicOptimization))
            {
                somethingChanged = new AlgebraicOptimization().RunOptimization(graph) || somethingChanged;
            }

            return somethingChanged;
        }
    }
}
