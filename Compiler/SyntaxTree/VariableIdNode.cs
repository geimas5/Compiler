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

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
