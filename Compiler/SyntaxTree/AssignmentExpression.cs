namespace Compiler.SyntaxTree
{
    public class AssignmentExpression : ExpressionNode
    {
        public AssignmentExpression(Location location, ExpressionNode leftSide, ExpressionNode rightSide)
            : base(location)
        {
            this.LeftSide = leftSide;
            this.RightSide = rightSide;
        }

        public ExpressionNode LeftSide { get; set; }

        public ExpressionNode RightSide { get; set; }
    }
}
