namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public abstract class Node
    {
        public Location Location { get; set; }

        public Node(Location location)
        {
            this.Location = location;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
        public abstract void Accept(IVisitor visitor);

        public abstract IEnumerable<Node> Children { get; } 

        public override string ToString()
        {
            return this.GetType().Name + this.Location + ' ';
        }
    }
}
