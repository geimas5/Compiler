namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    public abstract class Statement
    {
        private static int nextId;

        public Statement()
        {
            this.Id = nextId++;
            this.Predecessors = new List<Statement>();
        }

        public int Id { get; private set; }

        public ICollection<Statement> Predecessors { get; private set; }

        public Statement Next { get; set; }

        public BasicBlock BasicBlock { get; set; }
    }
}
