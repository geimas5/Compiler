namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    public abstract class Statement
    {
        protected bool Equals(Statement other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

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

        public IEnumerable<Statement> Successors
        {
            get
            {
                if (this.Next != null) yield return this.Next;
                if (this is JumpStatement)
                {
                    yield return ((JumpStatement)this).Target;
                }
                else if (this is BranchStatement)
                {
                    yield return ((BranchStatement)this).BranchTarget;
                }
            }
        } 

        public Statement Next { get; set; }

        public BasicBlock BasicBlock { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Statement)obj);
        }
    }
}
