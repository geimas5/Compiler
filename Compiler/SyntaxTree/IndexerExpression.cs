namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class IndexerExpression : ExpressionNode
    {
        public IndexerExpression(Location location, ExpressionNode name, ExpressionNode index)
            : base(location)
        {
            Trace.Assert(name != null);
            Trace.Assert(index != null);

            this.Name = name;
            this.Index = index;
        }

        public ExpressionNode Name { get; private set; }

        public ExpressionNode Index { get; private set; }

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
                yield return this.Name;
                yield return this.Index;
            }
        }
    }
}
