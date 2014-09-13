namespace Compiler.SyntaxTree
{
    using System;
    using System.Collections.Generic;

    public class FunctionDecleration : Node
    {
        public FunctionDecleration(
            Location location,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements)
            : base(location)
        {
            this.Name = name;
            this.Parameters = new List<VariableNode>();
            this.Statements = new List<StatementNode>();

            this.Parameters.AddRange(parameters);
            this.Statements.AddRange(statements);
        }

        public string Name { get; set; }

        public List<StatementNode> Statements { get; set; }

        public List<VariableNode> Parameters { get; set; }

        public override string ToString()
        {
            string text = "(FunctionDecleration " + this.Location + Environment.NewLine;

            foreach (var parameterNode in this.Parameters)
            {
                text += parameterNode.ToString();
            }

            return text + ")";
        }
    }
}
