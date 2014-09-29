namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    using Compiler.SymbolTable;

    public class VariableIdNode : Node
    {
        public string Name { get; set; }

        public VariableIdNode(Location location, string name)
            : base(location)
        {
            this.Name = name;
        }

        public ISymbol Symbol { get; set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IEnumerable<Node> Children
        {
            get
            {
                return new Node[0];
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
