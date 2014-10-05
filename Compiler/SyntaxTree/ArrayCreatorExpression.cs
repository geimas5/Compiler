namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    using Compiler.Common;

    public class ArrayCreatorExpression : CreatorExpression
    {
        public ArrayCreatorExpression(Location location, PrimitiveType type, IEnumerable<ExpressionNode> sizes)
            : base(location, type)
        {
            this.Sizes = new NotNullList<ExpressionNode>();

            this.Sizes.AddRange(sizes);
        }

        public IList<ExpressionNode> Sizes { get; private set; }

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
                foreach (var node in this.Sizes)
                {
                    yield return node;
                }
            }
        }
    }
}
