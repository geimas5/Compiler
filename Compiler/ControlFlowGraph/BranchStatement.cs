namespace Compiler.ControlFlowGraph
{
    public class BranchStatement : Statement
    {
        public BranchStatement(Statement branchTarget)
        {
            this.BranchTarget = branchTarget;
        }

        public Statement BranchTarget { get; private set; }
    }
}
