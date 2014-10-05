namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Compiler.Common;

    public class WhileStatement : StatementNode
    {
        public WhileStatement(Location location, ExpressionNode condition, IEnumerable<StatementNode> statements)
            : base(location)
        {
            Trace.Assert(condition != null);

            this.Body = new NotNullList<StatementNode>();

            this.Body.AddRange(statements);
            this.Condition = condition;
        }

        public ExpressionNode Condition { get; private set; }

        public IList<StatementNode> Body { get; private set; }

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
                yield return Condition;
                foreach (var node in this.Body)
                {
                    yield return node;
                }
            }
        }
    }
}
