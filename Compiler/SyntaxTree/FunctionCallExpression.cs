namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Compiler.Common;
    using Compiler.SymbolTable;

    public class FunctionCallExpression : ExpressionNode
    {
        public FunctionCallExpression(Location location, string name, IEnumerable<ExpressionNode> arguments)
            : base(location)
        {
            Trace.Assert(!string.IsNullOrEmpty(name));

            this.Arguments = new NotNullList<ExpressionNode>();

            this.Name = name;
            this.Arguments.AddRange(arguments);
        }

        public string Name { get; private set; }

        public IList<ExpressionNode> Arguments { get; private set; }

        public ISymbol Symbol { get; set; }

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
                return this.Arguments;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
