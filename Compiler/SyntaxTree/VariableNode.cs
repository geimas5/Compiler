
namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class VariableNode : Node
    {
        public VariableNode(Location location, TypeNode type, VariableIdNode name)
            : base(location)
        {
            Trace.Assert(type != null);
            Trace.Assert(name != null);

            this.Type = type;
            this.Name = name;
        }

        public TypeNode Type { get; private set; }

        public VariableIdNode Name { get; private set; }

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
