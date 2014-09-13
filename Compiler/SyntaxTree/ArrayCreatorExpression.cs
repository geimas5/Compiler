namespace Compiler.SyntaxTree
{
    public class ArrayCreatorExpression : CreatorExpression
    {
        public ArrayCreatorExpression(Location location, TypeNode type, ExpressionNode size)
            : base(location, type)
        {
            this.Size = size;
        }

        public ExpressionNode Size { get; set; }
    }
}
