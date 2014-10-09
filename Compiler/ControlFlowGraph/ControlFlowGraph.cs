namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ControlFlowGraph
    {
        public ControlFlowGraph()
        {
            this.Functions = new NotNullList<BasicBlock>();
        }

        public IList<BasicBlock> Functions { get; private set; }
    }
}
