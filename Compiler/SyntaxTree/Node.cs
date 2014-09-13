namespace Compiler.SyntaxTree
{
    public abstract class Node
    {
        public Location Location { get; set; }

        public Node(Location location)
        {
            this.Location = location;
        }
    }
}
