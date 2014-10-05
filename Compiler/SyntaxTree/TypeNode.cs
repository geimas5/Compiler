namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class TypeNode : Node
    {
        public TypeNode(Location location, Type type)
            : base(location)
        {
            Trace.Assert(type != null);

            this.Type = type;
        }

        public Type Type { get; private set; }

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
            return base.ToString() + string.Format("(primitiveType: {0} Dim: {1})", this.Type.PrimitiveType, this.Type.PrimitiveType);
        }
    }
}
