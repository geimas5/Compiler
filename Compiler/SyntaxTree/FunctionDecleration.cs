namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Compiler.Common;
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
            Trace.Assert(!string.IsNullOrEmpty(name));

            this.Name = name;
            this.Parameters = new NotNullList<VariableNode>();
            this.Statements = new NotNullList<StatementNode>();

            this.Parameters.AddRange(parameters);
            this.Statements.AddRange(statements);
        }

        public string Name { get; private set; }

        public IList<StatementNode> Statements { get; private set; }

        public IList<VariableNode> Parameters { get; private set; }

        public ISymbol Symbol { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Name: {0})", this.Name);
        }
    }
}
