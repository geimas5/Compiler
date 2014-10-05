namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class AssignmentExpression : ExpressionNode
    {
        public AssignmentExpression(Location location, ExpressionNode leftSide, ExpressionNode rightSide)
            : base(location)
        {
            Trace.Assert(leftSide != null);
            Trace.Assert(rightSide != null);

            this.LeftSide = leftSide;
            this.RightSide = rightSide;
        }

        public ExpressionNode LeftSide { get; private set; }

        public ExpressionNode RightSide { get; private set; }

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
                return new[] { this.LeftSide, this.RightSide };
            }
        }
    }
}
