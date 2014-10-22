namespace Compiler.Optimization
{
    using System.Linq;

    using Compiler.ControlFlowGraph;

    public abstract class OptimizerBase
    {
        private bool somethingChanged = false;

        public bool RunOptimization(ControlFlowGraph graph)
        {
            this.Init(graph);

             foreach (var block in graph.Functions.SelectMany(m => m.Value))
            {
                this.VisitBlock(block);
            }

            return this.somethingChanged;
        }

        public abstract void VisitBlock(BasicBlock block);

        protected void SetSomethingChanged()
        {
            this.somethingChanged = true;
        }

        protected virtual void Init(ControlFlowGraph graph)
        {
            
        }
    }
}
