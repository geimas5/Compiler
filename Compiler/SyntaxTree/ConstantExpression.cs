namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class ConstantExpression : ExpressionNode
    {
        public ConstantExpression(Location location, ConstantNode constant)
            : base(location)
        {
            Trace.Assert(constant != null);

            this.Constant = constant;
        }

        public ConstantNode Constant { get; private set; }

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
                return new[] { this.Constant };
            }
        }
    }
}
