namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ControlFlowGraph
    {
        public ControlFlowGraph()
        {
            this.Functions = new Dictionary<string, IList<BasicBlock>>();
            this.Strings = new Dictionary<string, string>();
        }

        public IDictionary<string, IList<BasicBlock>> Functions { get; private set; }

        public IDictionary<string, string> Strings { get; private set; }
    }
}
