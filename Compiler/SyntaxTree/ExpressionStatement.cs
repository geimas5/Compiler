namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    /// <summary>
    /// The expression statement.
    /// </summary>
    public class ExpressionStatement : StatementNode
    {
        public ExpressionStatement(Location location, ExpressionNode expression)
            : base(location)
        {
            this.Expression = expression;
        }

        public ExpressionNode Expression { get; set; }

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
                return new[] { this.Expression };
            }
        }
    }
}
