namespace Compiler.ControlFlowGraph
{
    using Compiler.SyntaxTree;

    public class UnaryOperatorStatement : Statement, IReturningStatement
    {
        public UnaryOperatorStatement(Destination @return, UnaryOperator @operator, Argument argument)
        {
            this.Return = @return;
            this.Operator = @operator;
            this.Argument = argument;
        }

        public Destination Return { get; private set; }

        public UnaryOperator Operator { get; set; }

        public Argument Argument { get; set; }
    }
}
