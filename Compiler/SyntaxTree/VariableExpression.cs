namespace Compiler.SyntaxTree
{
    public class VariableExpression : ExpressionNode
    {
        public VariableExpression(Location location, VariableIdNode variableId)
            : base(location)
        {
            this.VariableId = variableId;
        }

        public VariableIdNode VariableId { get; set; }
    }
}
