namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ControlFlowGraph
    {
        public ControlFlowGraph()
        {
            this.Functions = new NotNullList<ControlFlowNode>();
        }

        public IList<ControlFlowNode> Functions { get; private set; }
    }
}
