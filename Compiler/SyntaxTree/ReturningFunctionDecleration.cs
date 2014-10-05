namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class ReturningFunctionDecleration : FunctionDecleration
    {
        public ReturningFunctionDecleration(
            Location location,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements,
            TypeNode type)
            : base(location, name, parameters, statements)
        {
            Trace.Assert(type != null);

            this.Type = type;
        }

        public TypeNode Type { get; private set; }

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
                yield return this.Type;
                foreach (var node in this.Parameters)
                {
                    yield return node;
                }

                foreach (var node in this.Statements)
                {
                    yield return node;
                }
            }
        }
    }
}
