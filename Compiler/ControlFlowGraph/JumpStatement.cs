namespace Compiler.ControlFlowGraph
{
    public class JumpStatement : Statement
    {
        private Statement target;

        public JumpStatement(Statement target)
        {
            this.Target = target;
        }

        public Statement Target
        {
            get
            {
                return this.target;
            }
            set
            {
                if (this.target != null)
                    this.target.JumpSources.Remove(this);

                this.target = value;
                this.target.JumpSources.Add(this);
            }
        }

        public override string ToString()
        {
            return string.Format("Goto {0}", Target.Id);
        }
    }
}
