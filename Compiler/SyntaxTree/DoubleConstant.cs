namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class DoubleConstant : ConstantNode
    {
        public DoubleConstant(Location location, double value)
            : base(location)
        {
            this.Value = value;
        }

        public double Value { get; private set; }

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
            return base.ToString() + string.Format("(Value: {0})", this.Value);
        }
    }
}
