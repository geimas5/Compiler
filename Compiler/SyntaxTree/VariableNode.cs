
namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class VariableNode : Node
    {
        public VariableNode(Location location, TypeNode type, VariableIdNode name)
            : base(location)
        {
            this.Type = type;
            this.Name = name;
        }

        public TypeNode Type { get; set; }

        public VariableIdNode Name { get; set; }

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
                yield return this.Type;
                yield return this.Name;
            }
        }
    }
}
