namespace Compiler.SyntaxTree
{
    /// <summary>
    /// The expression statement.
    /// </summary>
    public class ExpressionStatement : StatementNode
    {
        public ExpressionStatement(Location location, ExpressionNode expression)
            : base(location)
        {
            this.Expression = expression;
        }

        public ExpressionNode Expression { get; set; }
    }
}
