namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.SymbolTable;

    public class ControlFlowGraph
    {
        public ControlFlowGraph()
        {
            this.Functions = new Dictionary<string, IList<BasicBlock>>();
            this.FunctionParameters = new Dictionary<string, IList<VariableSymbol>>();
            this.Strings = new Dictionary<string, string>();
        }

        public IDictionary<string, IList<BasicBlock>> Functions { get; private set; }

        public IDictionary<string, IList<VariableSymbol>> FunctionParameters { get; private set; }

        public IDictionary<string, string> Strings { get; private set; }
    }
}
