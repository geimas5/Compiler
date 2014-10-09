namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class Node
    {
        public Location Location { get; set; }

        protected Node(Location location)
        {
            Trace.Assert(location != null);

            this.Location = location;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
        public abstract void Accept(IVisitor visitor);

        public abstract IEnumerable<Node> Children { get; }

        public SymbolTable.SymbolTable SymbolTable { get; set; }

        public override string ToString()
        {
            return this.GetType().Name + this.Location + ' ';
        }
    }
}
