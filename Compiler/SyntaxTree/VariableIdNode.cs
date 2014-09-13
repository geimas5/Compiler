namespace Compiler.SyntaxTree
{
    public class VariableIdNode : Node
    {
        public string Name { get; set; }

        public VariableIdNode(Location location, string name)
            : base(location)
        {
            this.Name = name;
        }
    }
}
