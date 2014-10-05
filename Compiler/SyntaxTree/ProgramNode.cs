namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ProgramNode : Node
    {
        public ProgramNode(Location location)
            : base(location)
        {
            this.Functions = new NotNullList<FunctionDecleration>();
        }

        public IList<FunctionDecleration> Functions { get; private set; }

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
                foreach (var node in this.Functions)
                {
                    yield return node;
                }
            }
        }
    }
}
