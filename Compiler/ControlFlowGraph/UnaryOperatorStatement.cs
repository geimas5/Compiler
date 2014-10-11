namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    public class UnaryOperatorStatement : Statement, IReturningStatement
    {
        public UnaryOperatorStatement(VariableSymbol @return, UnaryOperator @operator, Argument argument)
        {
            this.Return = @return;
            this.Operator = @operator;
            this.Argument = argument;
        }

        public VariableSymbol Return { get; private set; }

        public UnaryOperator Operator { get; set; }

        public Argument Argument { get; set; }
    }
}
