namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Compiler.Common;

    public class ForStatement : StatementNode
    {
        public ForStatement(
            Location location,
            ExpressionNode initialization,
            ExpressionNode condition,
            ExpressionNode afterthought,
            IEnumerable<StatementNode> body)
            : base(location)
        {
            Trace.Assert(initialization != null);
            Trace.Assert(condition != null);

            this.Initialization = initialization;
            this.Condition = condition;
            this.Afterthought = afterthought;

            this.Body = new NotNullList<StatementNode>();
            this.Body.AddRange(body);
        }

        public ExpressionNode Initialization { get; private set; }

        public ExpressionNode Condition { get; private set; }

        public ExpressionNode Afterthought { get; private set; }

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
                yield return this.Initialization;
                yield return this.Condition;
                yield return this.Afterthought;
                foreach (var statementNode in Body)
                {
                    yield return statementNode;
                }
            }
        }
    }
}
