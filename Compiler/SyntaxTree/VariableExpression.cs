namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class VariableExpression : ExpressionNode
    {
        public VariableExpression(Location location, VariableIdNode variableId)
            : base(location)
        {
            Trace.Assert(variableId != null);

            this.VariableId = variableId;
        }

        public VariableIdNode VariableId { get; private set; }

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
                yield return this.VariableId;
            }
        }
    }
}
