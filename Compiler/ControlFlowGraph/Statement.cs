namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    public abstract class Statement
    {
        private static int nextId;

        public Statement()
        {
            this.Id = nextId++;
            this.JumpSources  = new List<Statement>();
        }

        public int Id { get; private set; }

        public Statement Previous { get; set; }

        public IList<Statement> JumpSources { get; set; }

        public IEnumerable<Statement> Predecessors
        {
            get
            {
                yield return this.Previous;
                foreach (var jumpSource in this.JumpSources)
                {
                    yield return jumpSource;
                }
            }
        }

        public Statement Next { get; set; }

        public BasicBlock BasicBlock { get; set; }
    }
}
