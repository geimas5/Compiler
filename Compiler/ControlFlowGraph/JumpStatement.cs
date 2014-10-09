namespace Compiler.ControlFlowGraph
{
    public class JumpStatement : Statement
    {
        public JumpStatement(Statement target)
        {
            this.Target = target;
        }

        public Statement Target { get; private set; }

        public override string ToString()
        {
            return string.Format("Goto {0}", Target.Id);
        }
    }
}
