namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    using Compiler.SymbolTable;

    public abstract class FunctionDecleration : Node
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

        public ISymbol Symbol { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
