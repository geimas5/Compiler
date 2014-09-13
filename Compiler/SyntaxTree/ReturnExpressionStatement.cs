namespace Compiler.SyntaxTree
{
    public class ReturnExpressionStatement : ReturnStatement
    {
        public ReturnExpressionStatement(Location location, ExpressionNode expression)
            : base(location)
        {
            this.Expression = expression;
        }

        public ExpressionNode Expression { get; set; }
    }
}
