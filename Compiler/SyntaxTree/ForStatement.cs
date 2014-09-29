namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

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
            this.Initialization = initialization;
            this.Condition = condition;
            this.Afterthought = afterthought;

            this.Body = new List<StatementNode>();
            this.Body.AddRange(body);
        }

        public ExpressionNode Initialization { get; set; }

        public ExpressionNode Condition { get; set; }

        public ExpressionNode Afterthought { get; set; }

        public List<StatementNode> Body { get; set; }

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
