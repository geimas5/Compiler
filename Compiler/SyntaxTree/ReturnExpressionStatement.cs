namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class ReturnExpressionStatement : ReturnStatement
    {
        public ReturnExpressionStatement(Location location, ExpressionNode expression)
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
                yield return this.Expression;
            }
        }
    }
}
