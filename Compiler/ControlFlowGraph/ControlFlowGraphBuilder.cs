namespace Compiler.ControlFlowGraph
{
    using Compiler.SyntaxTree;

    public class ControlFlowGraphBuilder : Visitor
    {
        private ControlFlowGraph controlFlowGraph;

        public ControlFlowGraph BuildGraph()
        {
            this.controlFlowGraph = new ControlFlowGraph();

            return this.controlFlowGraph;
        }

        public override void Visit(VoidFunctionDecleration node)
        {
            this.controlFlowGraph.Functions.Add(new ControlFlowNode());


            base.Visit(node);
        }
    }
}
