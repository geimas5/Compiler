namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class IfStatement : StatementNode
    {
        public IfStatement(Location location, ExpressionNode condition, IEnumerable<StatementNode> body, IEnumerable<StatementNode> els)
            : base(location)
        {
            this.Condition = condition;

            this.Body = new List<StatementNode>();
            this.ElseStatements = new List<StatementNode>();

            this.Body.AddRange(body);
            this.ElseStatements.AddRange(els);
        }


        public IfStatement(Location location, ExpressionNode condition, IEnumerable<StatementNode> body)
            : this(location, condition, body, new StatementNode[0])
        {
        }

        public ExpressionNode Condition { get; set; }

        public List<StatementNode> Body { get; set; }

        public List<StatementNode> ElseStatements { get; set; }
    }
}
