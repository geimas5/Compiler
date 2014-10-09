namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ControlFlowGraph
    {
        public ControlFlowGraph()
        {
            this.Functions = new NotNullList<IEnumerable<BasicBlock>>();
        }

        public IList<IEnumerable<BasicBlock>> Functions { get; private set; }
    }
}
