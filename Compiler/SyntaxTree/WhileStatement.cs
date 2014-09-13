namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class WhileStatement : StatementNode
    {
        public WhileStatement(Location location, ExpressionNode condition, IEnumerable<StatementNode> statements)
            : base(location)
        {
            this.Body = new List<StatementNode>();

            this.Body.AddRange(statements);
            this.Condition = condition;
        }

        public ExpressionNode Condition { get; set; }

        public List<StatementNode> Body { get; set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
