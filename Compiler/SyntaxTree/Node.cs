namespace Compiler.SyntaxTree
{
    public abstract class Node
    {
        public Location Location { get; set; }

        public Node(Location location)
        {
            this.Location = location;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
        public abstract void Accept(IVisitor visitor);

        public override string ToString()
        {
            return this.GetType().Name + this.Location + ' ';
        }
    }
}
