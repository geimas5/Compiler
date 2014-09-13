namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class FunctionCallExpression : ExpressionNode
    {
        public FunctionCallExpression(Location location, string name, IEnumerable<ExpressionNode> arguments)
            : base(location)
        {
            this.Arguments = new List<ExpressionNode>();

            this.Name = name;
            this.Arguments.AddRange(arguments);
        }

        public string Name { get; set; }

        public List<ExpressionNode> Arguments { get; set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
